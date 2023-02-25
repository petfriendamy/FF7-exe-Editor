using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Editor.Core;

namespace FF7exeEditor
{
    public partial class MainForm : Form
    {
        private EXEeditor editor, vanilla;
        private AccessoryData[] arrangedAccessoryList;
        private MateriaData[] arrangedMateriaList;
        private RadioButton[] csMateriaChecks, vMateriaChecks;
        private bool stopper = true;

        public MainForm()
        {
            InitializeComponent();
        }

        //update materia slot count
        private void UpdateMateriaSlots(Characters character)
        {
            //get data for selected character
            RadioButton[] materiaChecks;
            EquippedMateria[] equippedMateria;
            int weaponSlots, armorSlots;
            if (character == Characters.CaitSith)
            {
                materiaChecks = csMateriaChecks;
                equippedMateria = editor.CaitSith.Materia;
                weaponSlots = editor.CaitSith.Weapon.MateriaSlots;
                armorSlots = editor.CaitSith.Armor.MateriaSlots;
            }
            else
            {
                materiaChecks = vMateriaChecks;
                equippedMateria = editor.Vincent.Materia;
                weaponSlots = editor.Vincent.Weapon.MateriaSlots;
                armorSlots = editor.Vincent.Armor.MateriaSlots;
            }

            //update materia slots
            for (int i = 0; i < MateriaData.MATERIA_SLOTS; ++i)
            {
                if (i < 8) //weapon
                {
                    if (i < weaponSlots)
                    {
                        materiaChecks[i].Visible = true;
                    }
                    else //hide unused slots
                    {
                        materiaChecks[i].Visible = false;
                        equippedMateria[i].ClearSlot();
                        if (materiaChecks[i].Checked)
                        {
                            if (i > 0) //move back one
                            {
                                int j = i - 1;
                                while (j > 0 && !materiaChecks[j].Visible)
                                {
                                    --j;
                                }
                                materiaChecks[j].Checked = true;
                            }
                            else //switch to armor
                            {
                                int j = i + 8;
                                materiaChecks[j].Checked = true;
                            }
                        }
                    }
                }
                else //armor
                {
                    if (i < armorSlots + 8)
                    {
                        materiaChecks[i].Visible = true;
                    }
                    else //hide unused slots
                    {
                        materiaChecks[i].Visible = false;
                        equippedMateria[i].ClearSlot();
                        if (materiaChecks[i].Checked)
                        {
                            if (i > 8) //move back one
                            {
                                int j = i - 1;
                                while (j > 0 && !materiaChecks[j].Visible)
                                {
                                    --j;
                                }
                                materiaChecks[j].Checked = true;
                            }
                            else //switch to weapon
                            {
                                int j = i - 8;
                                materiaChecks[j].Checked = true;
                            }
                        }
                    }
                }
                //if all slots are hidden, disable materia editing
                bool temp = (materiaChecks[0].Visible || materiaChecks[8].Visible);
                if (character == Characters.CaitSith)
                {
                    comboBoxCSMateria.Enabled = temp;
                    numericCSAP.Enabled = (temp && comboBoxCSMateria.SelectedIndex > 0);
                }
                else
                {
                    comboBoxVMateria.Enabled = temp;
                    numericVAP.Enabled = (temp && comboBoxVMateria.SelectedIndex > 0);
                }
            }
        }

        //update selected materia ID + AP
        private void UpdateSelectedMateria(Characters character, int check)
        {
            stopper = true;
            EquippedMateria materia;
            ComboBox comboBox;
            NumericUpDown numeric;
            if (character == Characters.CaitSith)
            {
                materia = editor.CaitSith.Materia[check];
                comboBox = comboBoxCSMateria;
                numeric = numericCSAP;
            }
            else
            {
                materia = editor.Vincent.Materia[check];
                comboBox = comboBoxVMateria;
                numeric = numericVAP;
            }

            if (materia.MateriaID == null)
            {
                comboBox.SelectedIndex = 0;
                numeric.Value = 0;
                numeric.Enabled = false;
            }
            else
            {
                comboBox.SelectedItem = materia.MateriaID.MateriaName;
                if (materia.MateriaID.SpecialPropterties == MateriaSpecialProperties.Normal)
                {
                    numeric.Maximum = materia.MateriaID.MaxAP;
                    numeric.Value = materia.CurrentAP;
                    numeric.Enabled = true;
                }
                else
                {
                    numeric.Value = 0;
                    numeric.Enabled = false;
                }
            }
            stopper = false;
        }

        //returns selected materia slot
        private int GetSelectedMateriaSlot(Characters character)
        {
            RadioButton[] materiaChecks;
            if (character == Characters.CaitSith) { materiaChecks = csMateriaChecks; }
            else { materiaChecks = vMateriaChecks; }

            for (int i = 0; i < MateriaData.MATERIA_SLOTS; ++i) //check selected materia slot
            {
                if (materiaChecks[i].Checked)
                {
                    return i;
                }
            }
            return -1;
        }

        //change selected materia
        private void ChangeSelectedMateria(Characters character)
        {
            if (!stopper)
            {
                int slot, selected;
                EquippedMateria materia;
                NumericUpDown numeric;
                if (character == Characters.CaitSith)
                {
                    slot = GetSelectedMateriaSlot(Characters.CaitSith);
                    selected = comboBoxCSMateria.SelectedIndex;
                    materia = editor.CaitSith.Materia[slot];
                    numeric = numericCSAP;
                }
                else
                {
                    slot = GetSelectedMateriaSlot(Characters.Vincent);
                    selected = comboBoxVMateria.SelectedIndex;
                    materia = editor.Vincent.Materia[slot];
                    numeric = numericVAP;
                }

                //change materia in selected slot
                stopper = true;
                if (selected > 0)
                {
                    materia.MateriaID = arrangedMateriaList[selected - 1];
                    numeric.Enabled = true;
                    numeric.Maximum = materia.MateriaID.MaxAP;
                    numeric.Value = Math.Min(materia.CurrentAP, materia.MateriaID.MaxAP);

                }
                else
                {
                    materia.ClearSlot();
                    numeric.Value = 0;
                    numeric.Enabled = false;
                }
                stopper = false;
            }
        }

        //update a character's name
        private void ChangeName(TextBox textBox, int charID)
        {
            if (!stopper)
            {
                try
                {
                    editor.Names[charID].SetName(textBox.Text);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    stopper = true;
                    textBox.Text = editor.Names[charID].GetName();
                    stopper = false;
                }
            }
        }

        //sync controls with character data
        private void UpdateCharData()
        {
            stopper = true;

            //populate controls with names
            textBoxCloud.Text = editor.Names[0].GetName();
            textBoxBarret.Text = editor.Names[1].GetName();
            textBoxTifa.Text = editor.Names[2].GetName();
            textBoxAeris.Text = editor.Names[3].GetName();
            textBoxRedXIII.Text = editor.Names[4].GetName();
            textBoxYuffie.Text = editor.Names[5].GetName();
            textBoxCaitSith.Text = editor.Names[6].GetName();
            textBoxVincent.Text = editor.Names[7].GetName();
            textBoxCid.Text = editor.Names[8].GetName();
            textBoxChocobo.Text = editor.Names[9].GetName();

            //populate controls with Cait Sith data
            numericCSID.Value = editor.CaitSith.ID;
            numericCSLevel.Value = editor.CaitSith.Level;
            numericCSmaxHealth.Value = editor.CaitSith.MaxHealth;
            numericCScurrHealth.Value = editor.CaitSith.CurrHealth;
            numericCSmaxMana.Value = editor.CaitSith.MaxMana;
            numericCScurrMana.Value = editor.CaitSith.CurrMana;
            numericCSstr.Value = editor.CaitSith.Strength;
            numericCSvit.Value = editor.CaitSith.Vitality;
            numericCSmag.Value = editor.CaitSith.Magic;
            numericCSspr.Value = editor.CaitSith.Spirit;
            numericCSdex.Value = editor.CaitSith.Dexterity;
            numericCSlck.Value = editor.CaitSith.Luck;

            string temp;
            bool error = false;
            if (editor.CaitSith.Weapon != null)
            {
                temp = editor.CaitSith.Weapon.WeaponName;
                if (comboBoxCSWeapon.Items.Contains(temp))
                {
                    comboBoxCSWeapon.SelectedItem = temp;
                }
                else
                {
                    error = true;
                    comboBoxCSWeapon.SelectedIndex = 0;
                }
            }
            if (editor.CaitSith.Armor != null)
            {
                temp = editor.CaitSith.Armor.ArmorName;
                if (comboBoxCSArmor.Items.Contains(temp))
                {
                    comboBoxCSArmor.SelectedItem = temp;
                }
                else
                {
                    error = true;
                    comboBoxCSArmor.SelectedIndex = 0;
                }
            }
            if (editor.CaitSith.Accessory == null)
            {
                comboBoxCSAccessory.SelectedIndex = 0;
            }
            else
            {
                temp = editor.CaitSith.Accessory.AccessoryName;
                if (comboBoxCSAccessory.Items.Contains(temp))
                {
                    comboBoxCSAccessory.SelectedItem = temp;
                }
                else
                {
                    error = true;
                    comboBoxCSAccessory.SelectedIndex = 0;
                }
            }
            UpdateSelectedMateria(Characters.CaitSith, 0);

            //populate controls with Vincent data
            numericVID.Value = editor.Vincent.ID;
            numericVLevel.Value = editor.Vincent.Level;
            numericVcurrHealth.Value = editor.Vincent.CurrHealth;
            numericVmaxHealth.Value = editor.Vincent.MaxHealth;
            numericVcurrMana.Value = editor.Vincent.CurrMana;
            numericVmaxMana.Value = editor.Vincent.MaxMana;
            numericVstr.Value = editor.Vincent.Strength;
            numericVvit.Value = editor.Vincent.Vitality;
            numericVmag.Value = editor.Vincent.Magic;
            numericVspr.Value = editor.Vincent.Spirit;
            numericVdex.Value = editor.Vincent.Dexterity;
            numericVlck.Value = editor.Vincent.Luck;

            if (editor.Vincent.Weapon != null)
            {
                temp = editor.Vincent.Weapon.WeaponName;
                if (comboBoxVWeapon.Items.Contains(temp))
                {
                    comboBoxVWeapon.SelectedItem = temp;
                }
                else
                {
                    error = true;
                    comboBoxVWeapon.SelectedIndex = 0;
                }
            }
            if (editor.Vincent.Armor != null)
            {
                temp = editor.Vincent.Armor.ArmorName;
                if (comboBoxVArmor.Items.Contains(temp))
                {
                    comboBoxVArmor.SelectedItem = temp;
                }
                else
                {
                    error = true;
                    comboBoxVArmor.SelectedIndex = 0;
                }
            }
            if (editor.Vincent.Accessory == null)
            {
                comboBoxVAccessory.SelectedIndex = 0;
            }
            else
            {
                temp = editor.Vincent.Accessory.AccessoryName;
                if (comboBoxVAccessory.Items.Contains(temp))
                {
                    comboBoxVAccessory.SelectedItem = temp;
                }
                else
                {
                    error = true;
                    comboBoxVAccessory.SelectedIndex = 0;
                }
            }
            UpdateSelectedMateria(Characters.Vincent, 0);

            //check if there was an error
            if (error)
            {
                MessageBox.Show("An error was found in the character data. Some data may not have loaded correctly.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //on load
        private void MainForm_Load(object sender, EventArgs e)
        {
            //get materia radio buttons as array
            csMateriaChecks = new RadioButton[MateriaData.MATERIA_SLOTS]
            {
                radioCSW1, radioCSW2, radioCSW3, radioCSW4, radioCSW5, radioCSW6, radioCSW7, radioCSW8,
                radioCSA1, radioCSA2, radioCSA3, radioCSA4, radioCSA5, radioCSA6, radioCSA7, radioCSA8
            };
            vMateriaChecks = new RadioButton[MateriaData.MATERIA_SLOTS]
            {
                radioVW1, radioVW2, radioVW3, radioVW4, radioVW5, radioVW6, radioVW7, radioVW8,
                radioVA1, radioVA2, radioVA3, radioVA4, radioVA5, radioVA6, radioVA7, radioVA8
            };

            //arrange accessory list
            var aLinq =
                from a in GameData.ACCESSORY_LIST
                orderby a.AccessoryName
                select a;
            arrangedAccessoryList = aLinq.ToArray();

            //arrange materia list
            var mLinq =
                from m in GameData.MATERIA_LIST
                orderby m.MateriaType, m.SpecialPropterties != MateriaSpecialProperties.Master, m.MateriaName
                select m;
            arrangedMateriaList = mLinq.ToArray();

            //populate comboboxes
            foreach (var csWeapon in GameData.CAIT_SITH_WEAPON_LIST)
            {
                comboBoxCSWeapon.Items.Add(csWeapon.WeaponName);
            }
            foreach (var vWeapon in GameData.VINCENT_WEAPON_LIST)
            {
                comboBoxVWeapon.Items.Add(vWeapon.WeaponName);
            }
            foreach (var armor in GameData.ARMOR_LIST)
            {
                comboBoxCSArmor.Items.Add(armor.ArmorName);
                comboBoxVArmor.Items.Add(armor.ArmorName);
            }
            comboBoxCSAccessory.Items.Add("None");
            comboBoxVAccessory.Items.Add("None");
            foreach (var accessory in arrangedAccessoryList)
            {
                comboBoxCSAccessory.Items.Add(accessory.AccessoryName);
                comboBoxVAccessory.Items.Add(accessory.AccessoryName);
            }
            comboBoxCSMateria.Items.Add("None");
            comboBoxVMateria.Items.Add("None");
            foreach (var materia in arrangedMateriaList)
            {
                comboBoxCSMateria.Items.Add(materia.MateriaName);
                comboBoxVMateria.Items.Add(materia.MateriaName);
            }

            //get data from EXE
            DialogResult result;
            string path;

            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Final Fantasy VII executable|ff7_en.exe;ff7_es.exe;ff7_fr.exe;ff7_de.exe;ff7.exe";
                result = openDialog.ShowDialog();
                path = openDialog.FileName;
            }

            if (result != DialogResult.OK)
            {
                Application.Exit();
            }
            else
            {
                try
                {
                    editor = new EXEeditor(path);
                    UpdateCharData();
                    if (editor.Language != Language.English)
                    {
                        buttonHext.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }

            //get vanilla data for comparison
            vanilla = new EXEeditor(null);
            vanilla.ReadBytes(Properties.Resources.chardata);
        }

        //change character ID
        private void numericCSID_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.ID = (byte)numericCSID.Value;
        }

        private void numericVID_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.ID = (byte)numericVID.Value;
        }

        //change level
        private void numericCSLevel_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Level = (byte)numericCSLevel.Value;
        }

        private void numericVLevel_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Level = (byte)numericVLevel.Value;
        }

        //change HP
        private void numericCScurrHealth_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                if (numericCScurrHealth.Value > editor.CaitSith.MaxHealth)
                {
                    numericCScurrHealth.Value = editor.CaitSith.MaxHealth;
                }
                editor.CaitSith.CurrHealth = (ushort)numericCScurrHealth.Value;
            }
        }

        private void numericCSmaxHealth_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                var maxHealth = (ushort)numericCSmaxHealth.Value;
                if (maxHealth < editor.CaitSith.CurrHealth)
                {
                    numericCScurrHealth.Value = maxHealth;
                    editor.CaitSith.CurrHealth = maxHealth;
                }
                editor.CaitSith.MaxHealth = maxHealth;
            }
        }

        private void numericVcurrHealth_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                if (numericVcurrHealth.Value > editor.Vincent.MaxHealth)
                {
                    numericVcurrHealth.Value = editor.Vincent.MaxHealth;
                }
                editor.Vincent.CurrHealth = (ushort)numericVcurrHealth.Value;
            }
        }

        private void numericVmaxHealth_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                var maxHealth = (ushort)numericVmaxHealth.Value;
                if (maxHealth < editor.Vincent.CurrHealth)
                {
                    numericVcurrHealth.Value = maxHealth;
                    editor.Vincent.CurrHealth = maxHealth;
                }
                editor.Vincent.MaxHealth = maxHealth;
            }
        }

        //change MP
        private void numericCScurrMana_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                if (numericCScurrMana.Value > editor.CaitSith.MaxMana)
                {
                    numericCScurrMana.Value = editor.CaitSith.MaxMana;
                }
                editor.CaitSith.CurrMana = (ushort)numericCScurrMana.Value;
            }
        }

        private void numericCSmaxMana_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                var maxMana = (ushort)numericCSmaxMana.Value;
                if (maxMana < editor.CaitSith.CurrMana)
                {
                    numericCScurrMana.Value = maxMana;
                    editor.CaitSith.CurrMana = maxMana;
                }
                editor.CaitSith.MaxMana = maxMana;
            }
        }

        private void numericVcurrMana_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                if (numericVcurrMana.Value > editor.Vincent.MaxMana)
                {
                    numericVcurrMana.Value = editor.Vincent.MaxMana;
                }
                editor.Vincent.CurrMana = (ushort)numericVcurrMana.Value;
            }
        }

        private void numericVmaxMana_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                var maxMana = (ushort)numericVmaxMana.Value;
                if (maxMana < editor.Vincent.CurrMana)
                {
                    numericVcurrMana.Value = maxMana;
                    editor.Vincent.CurrMana = maxMana;
                }
                editor.Vincent.MaxMana = maxMana;
            }
        }

        //change stats
        private void numericCSstr_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Strength = (byte)numericCSstr.Value;
        }

        private void numericCSvit_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Vitality = (byte)numericCSvit.Value;
        }

        private void numericCSmag_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Magic = (byte)numericCSmag.Value;
        }

        private void numericCSspr_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Spirit = (byte)numericCSspr.Value;
        }

        private void numericCSdex_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Dexterity = (byte)numericCSdex.Value;
        }

        private void numericCSlck_ValueChanged(object sender, EventArgs e)
        {
            editor.CaitSith.Luck = (byte)numericCSlck.Value;
        }

        private void numericVstr_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Strength = (byte)numericVstr.Value;
        }

        private void numericVvit_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Vitality = (byte)numericVvit.Value;
        }

        private void numericVmag_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Magic = (byte)numericVmag.Value;
        }

        private void numericVspr_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Spirit = (byte)numericVspr.Value;
        }

        private void numericVdex_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Dexterity = (byte)numericVdex.Value;
        }

        private void numericVlck_ValueChanged(object sender, EventArgs e)
        {
            editor.Vincent.Luck = (byte)numericVlck.Value;
        }

        //change weapon
        private void comboBoxCSWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            int w = comboBoxCSWeapon.SelectedIndex;
            if (w >= 0 && w < GameData.CAIT_SITH_WEAPON_LIST.Length)
            {
                editor.CaitSith.Weapon = GameData.CAIT_SITH_WEAPON_LIST[comboBoxCSWeapon.SelectedIndex];
                UpdateMateriaSlots(Characters.CaitSith);
            }
        }

        private void comboBoxVWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            int w = comboBoxVWeapon.SelectedIndex;
            if (w >= 0 && w < GameData.VINCENT_WEAPON_LIST.Length)
            {
                editor.Vincent.Weapon = GameData.VINCENT_WEAPON_LIST[comboBoxVWeapon.SelectedIndex];
                UpdateMateriaSlots(Characters.Vincent);
            }
        }

        //change armor
        private void comboBoxCSArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxCSArmor.SelectedIndex;
            if (a >= 0 && a < GameData.ARMOR_LIST.Length)
            {
                editor.CaitSith.Armor = GameData.ARMOR_LIST[comboBoxCSArmor.SelectedIndex];
                UpdateMateriaSlots(Characters.CaitSith);
            }
        }

        private void comboBoxVArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxVArmor.SelectedIndex;
            if (a >= 0 && a < GameData.ARMOR_LIST.Length)
            {
                editor.Vincent.Armor = GameData.ARMOR_LIST[comboBoxVArmor.SelectedIndex];
                UpdateMateriaSlots(Characters.Vincent);
            }
        }

        //change accesssory
        private void comboBoxCSAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxCSAccessory.SelectedIndex;
            if (a > 0 && a < GameData.ACCESSORY_LIST.Length)
            {
                editor.CaitSith.Accessory = arrangedAccessoryList[comboBoxCSAccessory.SelectedIndex - 1];
            }
            else
            {
                editor.CaitSith.Accessory = null;
            }
        }

        private void comboBoxVAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxVAccessory.SelectedIndex;
            if (a > 0 && a < GameData.ACCESSORY_LIST.Length)
            {
                editor.Vincent.Accessory = arrangedAccessoryList[comboBoxVAccessory.SelectedIndex - 1];
            }
            else
            {
                editor.Vincent.Accessory = null;
            }
        }

        //change materia slot
        private void radioCSMateria_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < MateriaData.MATERIA_SLOTS; ++i)
            {
                if (sender == csMateriaChecks[i])
                {
                    UpdateSelectedMateria(Characters.CaitSith, i);
                    break;
                }
            }
        }

        private void radioVMateria_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < MateriaData.MATERIA_SLOTS; ++i)
            {
                if (sender == vMateriaChecks[i])
                {
                    UpdateSelectedMateria(Characters.Vincent, i);
                    break;
                }
            }
        }

        //change selected materia
        private void comboBoxCSMateria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedMateria(Characters.CaitSith);
        }

        private void comboBoxVMateria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedMateria(Characters.Vincent);
        }

        //change AP
        private void numericCSAP_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                int slot = GetSelectedMateriaSlot(Characters.CaitSith),
                ap = (int)numericCSAP.Value;

                if (editor.CaitSith.Materia[slot].MateriaID != null &&
                        editor.CaitSith.Materia[slot].CurrentAP != ap)
                {
                    editor.CaitSith.Materia[slot].CurrentAP = ap;
                }
            }
        }

        private void numericVAP_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper)
            {
                int slot = GetSelectedMateriaSlot(Characters.Vincent),
                ap = (int)numericVAP.Value;

                if (editor.Vincent.Materia[slot].MateriaID != null &&
                        editor.Vincent.Materia[slot].CurrentAP != ap)
                {
                    editor.Vincent.Materia[slot].CurrentAP = ap;
                }
            }
        }

        //update character names
        private void textBoxCloud_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxCloud, 0);
        }

        private void textBoxBarret_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxBarret, 1);
        }

        private void textBoxTifa_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxTifa, 2);
        }

        private void textBoxAeris_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxAeris, 3);
        }

        private void textBoxRedXIII_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxRedXIII, 4);
        }

        private void textBoxYuffie_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxYuffie, 5);
        }

        private void textBoxCaitSith_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxCaitSith, 6);
        }

        private void textBoxVincent_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxVincent, 7);
        }

        private void textBoxCid_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxCid, 8);
        }

        private void textBoxChocobo_TextChanged(object sender, EventArgs e)
        {
            ChangeName(textBoxChocobo, 9);
        }

        //load char data from a file
        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string path;
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "DAT file|*.dat";
                result = openDialog.ShowDialog();
                path = openDialog.FileName;
            }

            if (result == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        editor.ReadFile(path);
                        UpdateCharData();
                    }
                    else
                    {
                        MessageBox.Show($"File could not be found.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"{ex.Message} ({ex.InnerException.Message})", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //save char data to a file
        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string path;
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "DAT file|*.dat";
                result = saveDialog.ShowDialog();
                path = saveDialog.FileName;
            }

            if (result == DialogResult.OK)
            {
                try
                {
                    editor.WriteFile(path);
                    MessageBox.Show("Saved successfully!");
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"{ex.Message} ({ex.InnerException.Message})", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //checks if mouse is over certain controls
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            var control = GetChildAtPoint(e.Location);
            if (control == null)
            {
                toolTip.ShowAlways = false;
            }
            else
            {
                if (control == buttonHext && editor.Language != Language.English)
                {
                    if (!toolTip.ShowAlways)
                    {
                        toolTip.ShowAlways = true;
                        toolTip.Show("Currently unavailable for this language.", control, control.Width / 2, control.Height / 2);
                    }
                }
                else
                {
                    toolTip.ShowAlways = false;
                }
            }
        }

        //create a Hext file
        private void buttonHext_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string path;
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text file|*.txt";
                result = saveDialog.ShowDialog();
                path = saveDialog.FileName;
            }

            if (result == DialogResult.OK)
            {
                try
                {
                    editor.CreateHextFile(path, vanilla);
                    MessageBox.Show("Saved successfully!");
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"{ex.Message} ({ex.InnerException.Message})", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //update EXE
        private void buttonSaveEXE_Click(object sender, EventArgs e)
        {
            try
            {
                editor.WriteEXE();
                MessageBox.Show("Saved successfully!");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"{ex.Message} ({ex.InnerException.Message})", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
