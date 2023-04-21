using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Editor.Core;

namespace FF7exeEditor
{
    public partial class MainForm : Form
    {
        private const int ITEM_OFFSET = GameData.ITEM_END - GameData.ITEM_COUNT;

        private EXEeditor? editor, vanilla;
        private ReadOnlyCollection<WeaponData> csWeaponList, vWeaponList;
        private ReadOnlyCollection<AccessoryData> arrangedAccessoryList;
        private ReadOnlyCollection<MateriaData> arrangedMateriaList;
        private RadioButton[] csMateriaChecks, vMateriaChecks;
        private TextBox[] nameTextBoxes;
        private ComboBox[] shopItemList;
        private bool stopper = true;

        public MainForm()
        {
            InitializeComponent();

            int i;

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

            //get name textboxes as array
            nameTextBoxes = new TextBox[10]
            {
                textBoxCloud, textBoxBarret, textBoxTifa, textBoxAeris, textBoxRedXIII, textBoxYuffie,
                textBoxCaitSith, textBoxVincent, textBoxCid, textBoxChocobo
            };

            //get shop comboboxes as array
            shopItemList = new ComboBox[ShopInventory.SHOP_ITEM_MAX]
            {
                comboBoxShopItem1, comboBoxShopItem2, comboBoxShopItem3, comboBoxShopItem4, comboBoxShopItem5,
                comboBoxShopItem6, comboBoxShopItem7, comboBoxShopItem8, comboBoxShopItem9, comboBoxShopItem10
            };

            //get Cait Sith weapons
            var cLinq =
                from w in GameData.WEAPON_LIST
                where w.EquipableBy == Characters.CaitSith
                select w;
            csWeaponList = cLinq.ToArray().AsReadOnly();

            //get Vincent weapons
            var vLinq =
                from w in GameData.WEAPON_LIST
                where w.EquipableBy == Characters.Vincent
                select w;
            vWeaponList = vLinq.ToArray().AsReadOnly();

            //arrange accessory list
            var aLinq =
                from a in GameData.ACCESSORY_LIST
                orderby a.Name
                select a;
            arrangedAccessoryList = aLinq.ToArray().AsReadOnly();

            //arrange materia list
            var mLinq =
                from m in GameData.MATERIA_LIST
                where m.SpecialPropterties != MateriaSpecialProperties.Unused
                orderby m.MateriaType, m.SpecialPropterties != MateriaSpecialProperties.Master, m.Name
                select m;
            arrangedMateriaList = mLinq.ToArray().AsReadOnly();

            //populate comboboxes
            SuspendOrResumeComboBoxes(tabControlMain, false);
            foreach (var item in GameData.ITEM_LIST)
            {
                foreach (var shopItem in shopItemList)
                {
                    shopItem.Items.Add(item.Name);
                    shopItem.SelectedIndex = 0;
                }
            }
            foreach (var weapon in GameData.WEAPON_LIST)
            {
                if (weapon.EquipableBy == Characters.CaitSith)
                {
                    comboBoxCSWeapon.Items.Add(weapon.Name);
                }
                if (weapon.EquipableBy == Characters.Vincent)
                {
                    comboBoxVWeapon.Items.Add(weapon.Name);
                }
                foreach (var shopItem in shopItemList)
                {
                    shopItem.Items.Add(weapon.Name);
                }
            }
            foreach (var armor in GameData.ARMOR_LIST)
            {
                comboBoxCSArmor.Items.Add(armor.Name);
                comboBoxVArmor.Items.Add(armor.Name);
                foreach (var shopItem in shopItemList)
                {
                    shopItem.Items.Add(armor.Name);
                }
            }
            comboBoxCSAccessory.Items.Add("None");
            comboBoxVAccessory.Items.Add("None");
            foreach (var accessory in arrangedAccessoryList)
            {
                comboBoxCSAccessory.Items.Add(accessory.Name);
                comboBoxVAccessory.Items.Add(accessory.Name);
                foreach (var shopItem in shopItemList)
                {
                    shopItem.Items.Add(accessory.Name);
                }
            }
            comboBoxCSMateria.Items.Add("None");
            comboBoxVMateria.Items.Add("None");
            foreach (var materia in arrangedMateriaList)
            {
                comboBoxCSMateria.Items.Add(materia.Name);
                comboBoxVMateria.Items.Add(materia.Name);
                foreach (var shopItem in shopItemList)
                {
                    shopItem.Items.Add(materia.Name);
                }
            }

            //add shop data
            for (i = 0; i < EXEeditor.NUM_SHOPS; ++i)
            {
                if (GameData.SHOP_NAMES.ContainsKey(i))
                {
                    comboBoxShopIndex.Items.Add(GameData.SHOP_NAMES[i]);
                }
                else
                {
                    comboBoxShopIndex.Items.Add($"[Shop ID {i}]");
                }
            }

            //resume combo boxes
            SuspendOrResumeComboBoxes(tabControlMain, true);

            //set numeric max values
            numericItemPrice.Maximum = uint.MaxValue;
            numericMateriaPrice.Maximum = uint.MaxValue;
        }



        //on form open
        private void MainForm_Shown(object sender, EventArgs e)
        {
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
                    UpdateFormData();
                    if (editor.Language != Language.English)
                    {
                        buttonHext.Enabled = false;
                    }
                    comboBoxShopIndex.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }

        //sync controls with EXE data
        private void UpdateFormData()
        {
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }
            stopper = true;
            bool error = false;
            string temp;
            int i;

            //set AP price multiplier
            numericMateriaAPPriceMultiplier.Value = editor.APPriceMultiplier;

            //populate controls with character names
            for (i = 0; i < 10; ++i)
            {
                nameTextBoxes[i].Text = editor.CharacterNames[i].ToString();
            }

            //populate controls with Cait Sith data
            if (editor.CaitSith != null)
            {
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
                trackBarCSLimitBar.Value = editor.CaitSith.LimitBar;
                numericCSLimitLevel.Value = editor.CaitSith.LimitLevel;

                if (editor.CaitSith.Weapon != null)
                {
                    if (csWeaponList.Contains(editor.CaitSith.Weapon))
                    {
                        comboBoxCSWeapon.SelectedItem = editor.CaitSith.Weapon.Name;
                    }
                    else if (GameData.WEAPON_LIST.Contains(editor.CaitSith.Weapon))
                    {
                        checkBoxCSAllowAll.Checked = true;
                        comboBoxCSWeapon.SuspendLayout();
                        comboBoxCSWeapon.Items.Clear();
                        foreach (var w in GameData.WEAPON_LIST)
                        {
                            comboBoxCSWeapon.Items.Add(w.Name);
                        }
                        comboBoxCSWeapon.SelectedIndex = GameData.WEAPON_LIST.IndexOf(editor.CaitSith.Weapon);
                        comboBoxCSWeapon.ResumeLayout();
                    }
                    else
                    {
                        error = true;
                        comboBoxCSWeapon.SelectedIndex = 0;
                    }
                }
                if (editor.CaitSith.Armor != null)
                {
                    temp = editor.CaitSith.Armor.Name;
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
                    temp = editor.CaitSith.Accessory.Name;
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
            }

            //populate controls with Vincent data
            if (editor.Vincent != null)
            {
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
                trackBarVLimitBar.Value = editor.Vincent.LimitBar;
                numericVLimitLevel.Value = editor.Vincent.LimitLevel;

                if (editor.Vincent.Weapon != null)
                {
                    if (vWeaponList.Contains(editor.Vincent.Weapon))
                    {
                        comboBoxVWeapon.SelectedItem = editor.Vincent.Weapon.Name;
                    }
                    else if (GameData.WEAPON_LIST.Contains(editor.Vincent.Weapon))
                    {
                        checkBoxVAllowAll.Checked = true;
                        comboBoxVWeapon.SuspendLayout();
                        comboBoxVWeapon.Items.Clear();
                        foreach (var w in GameData.WEAPON_LIST)
                        {
                            comboBoxVWeapon.Items.Add(w.Name);
                        }
                        comboBoxVWeapon.SelectedIndex = GameData.WEAPON_LIST.IndexOf(editor.Vincent.Weapon);
                        comboBoxVWeapon.ResumeLayout();
                    }
                    else
                    {
                        error = true;
                        comboBoxVWeapon.SelectedIndex = 0;
                    }
                }
                if (editor.Vincent.Armor != null)
                {
                    temp = editor.Vincent.Armor.Name;
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
                    temp = editor.Vincent.Accessory.Name;
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
            }

            //suspend layouts
            listBoxItemPrices.SuspendLayout();
            listBoxMateriaPrices.SuspendLayout();
            comboBoxShopType.SuspendLayout();

            //clear items
            int shopType = comboBoxShopType.SelectedIndex;
            if (shopType < 0) { shopType = 0; }
            listBoxItemPrices.Items.Clear();
            listBoxMateriaPrices.Items.Clear();
            comboBoxShopType.Items.Clear();

            //populate listbox with item prices
            for (i = 0; i < GameData.ITEM_COUNT; ++i)
            {
                listBoxItemPrices.Items.Add($"{GameData.ITEM_LIST[i].Name} - {editor.ItemPrices[i]}");
            }
            for (i = 0; i < GameData.WEAPON_COUNT; ++i)
            {
                listBoxItemPrices.Items.Add($"{GameData.WEAPON_LIST[i].Name} - {editor.WeaponPrices[i]}");
            }
            for (i = 0; i < GameData.ARMOR_COUNT; ++i)
            {
                listBoxItemPrices.Items.Add($"{GameData.ARMOR_LIST[i].Name} - {editor.ArmorPrices[i]}");
            }
            for (i = 0; i < GameData.ACCESSORY_COUNT; ++i)
            {
                int pos = GameData.ACCESSORY_LIST.IndexOf(arrangedAccessoryList[i]);
                if (pos != -1)
                {
                    listBoxItemPrices.Items.Add($"{arrangedAccessoryList[i].Name} - {editor.AccessoryPrices[pos]}");
                }
            }

            //populate listbox with materia prices
            for (i = 0; i < arrangedMateriaList.Count; ++i)
            {
                int pos = GameData.MATERIA_LIST.IndexOf(arrangedMateriaList[i]);
                if (pos != -1)
                {
                    listBoxMateriaPrices.Items.Add($"{arrangedMateriaList[i].Name} - {editor.MateriaPrices[pos]}");
                }
            }

            //populate combobox with shop names
            foreach (var s in editor.ShopNames)
            {
                comboBoxShopType.Items.Add(s.ToString());
            }
            comboBoxShopType.SelectedIndex = shopType;

            //resume layouts
            listBoxItemPrices.ResumeLayout();
            listBoxMateriaPrices.ResumeLayout();
            comboBoxShopType.ResumeLayout();

            //check if there was an error
            if (error)
            {
                MessageBox.Show("Errors were found in the EXE. Some data may not have loaded correctly.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //suspend comboboxes so we can add stuff to them (or resume when done)
        private void SuspendOrResumeComboBoxes(Control control, bool resume)
        {
            if (control is TabControl)
            {
                var tc = control as TabControl;
                if (tc != null)
                {
                    for (int i = 0; i < tc.TabCount; ++i)
                    {
                        SuspendOrResumeComboBoxes(tc.TabPages[i], resume);
                    }
                }
            }
            else if (control is GroupBox)
            {
                for (int i = 0; i < control.Controls.Count; ++i)
                {
                    SuspendOrResumeComboBoxes(control.Controls[i], resume);
                }
            }
            else if (control is ComboBox)
            {
                var cb = control as ComboBox;
                if (resume) { cb?.ResumeLayout(); }
                else { cb?.SuspendLayout(); }
            }
        }

        //update materia slot count
        private void UpdateMateriaSlots(Characters character)
        {
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }

            //get data for selected character
            RadioButton[]? materiaChecks = null;
            EquippedMateria[]? equippedMateria = null;
            int weaponSlots = 0, armorSlots = 0;
            if (character == Characters.CaitSith && editor.CaitSith != null)
            {
                materiaChecks = csMateriaChecks;
                equippedMateria = editor.CaitSith.Materia;
                if (editor.CaitSith.Weapon != null)
                {
                    weaponSlots = editor.CaitSith.Weapon.MateriaSlots;
                }
                if (editor.CaitSith.Armor != null)
                {
                    armorSlots = editor.CaitSith.Armor.MateriaSlots;
                }
            }
            else if (editor.Vincent != null)
            {
                materiaChecks = vMateriaChecks;
                equippedMateria = editor.Vincent.Materia;
                if (editor.Vincent.Weapon != null)
                {
                    weaponSlots = editor.Vincent.Weapon.MateriaSlots;
                }
                if (editor.Vincent.Armor != null)
                {
                    armorSlots = editor.Vincent.Armor.MateriaSlots;
                }
            }

            //update materia slots
            if (materiaChecks != null && equippedMateria != null)
            {
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
        }

        //update selected materia ID + AP
        private void UpdateSelectedMateria(Characters character, int check)
        {
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }

            stopper = true;
            EquippedMateria? materia = null;
            ComboBox? comboBox = null;
            NumericUpDown? numeric = null;
            if (character == Characters.CaitSith && editor.CaitSith != null)
            {
                materia = editor.CaitSith.Materia[check];
                comboBox = comboBoxCSMateria;
                numeric = numericCSAP;
            }
            else if (editor.Vincent != null)
            {
                materia = editor.Vincent.Materia[check];
                comboBox = comboBoxVMateria;
                numeric = numericVAP;
            }

            if (materia != null && comboBox != null && numeric != null)
            {
                if (materia.MateriaID == null)
                {
                    comboBox.SelectedIndex = 0;
                    numeric.Value = 0;
                    numeric.Enabled = false;
                }
                else
                {
                    comboBox.SelectedItem = materia.MateriaID.Name;
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
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }
            if (!stopper)
            {
                int slot = 0, selected = 0;
                EquippedMateria? materia = null;
                NumericUpDown? numeric = null;
                if (character == Characters.CaitSith && editor.CaitSith != null)
                {
                    slot = GetSelectedMateriaSlot(Characters.CaitSith);
                    selected = comboBoxCSMateria.SelectedIndex;
                    materia = editor.CaitSith.Materia[slot];
                    numeric = numericCSAP;
                }
                else if (editor.Vincent != null)
                {
                    slot = GetSelectedMateriaSlot(Characters.Vincent);
                    selected = comboBoxVMateria.SelectedIndex;
                    materia = editor.Vincent.Materia[slot];
                    numeric = numericVAP;
                }

                //change materia in selected slot
                if (materia != null && numeric != null)
                {
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
        }

        //update a character's name
        private void ChangeName(TextBox textBox, int charID)
        {
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }
            if (!stopper)
            {
                try
                {
                    editor.CharacterNames[charID].SetName(textBox.Text);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    stopper = true;
                    textBox.Text = editor.CharacterNames[charID].ToString();
                    stopper = false;
                }
            }
        }

        //change character ID
        private void numericCSID_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.ID = (byte)numericCSID.Value;
            }
        }

        private void numericVID_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.ID = (byte)numericVID.Value;
            }
        }

        //change level
        private void numericCSLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Level = (byte)numericCSLevel.Value;
            }
        }

        private void numericVLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Level = (byte)numericVLevel.Value;
            }
        }

        //change HP
        private void numericCScurrHealth_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
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
            if (!stopper && editor != null && editor.CaitSith != null)
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
            if (!stopper && editor != null && editor.Vincent != null)
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
            if (!stopper && editor != null && editor.Vincent != null)
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
            if (!stopper && editor != null && editor.CaitSith != null)
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
            if (!stopper && editor != null && editor.CaitSith != null)
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
            if (!stopper && editor != null && editor.Vincent != null)
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
            if (!stopper && editor != null && editor.Vincent != null)
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
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Strength = (byte)numericCSstr.Value;
            }
        }

        private void numericCSvit_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Vitality = (byte)numericCSvit.Value;
            }
        }

        private void numericCSmag_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Magic = (byte)numericCSmag.Value;
            }
        }

        private void numericCSspr_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Spirit = (byte)numericCSspr.Value;
            }
        }

        private void numericCSdex_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Dexterity = (byte)numericCSdex.Value;
            }
        }

        private void numericCSlck_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Luck = (byte)numericCSlck.Value;
            }
        }

        private void numericVstr_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Strength = (byte)numericVstr.Value;
            }
        }

        private void numericVvit_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Vitality = (byte)numericVvit.Value;
            }
        }

        private void numericVmag_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Magic = (byte)numericVmag.Value;
            }
        }

        private void numericVspr_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Spirit = (byte)numericVspr.Value;
            }
        }

        private void numericVdex_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Dexterity = (byte)numericVdex.Value;
            }
        }

        private void numericVlck_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Luck = (byte)numericVlck.Value;
            }
        }

        //change limit
        private void trackBarCSLimitBar_Scroll(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.LimitBar = (byte)trackBarCSLimitBar.Value;
            }
        }

        private void numericCSLimitLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.LimitLevel = (byte)numericCSLimitLevel.Value;
            }
        }

        private void trackBarVLimitBar_Scroll(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.LimitBar = (byte)trackBarVLimitBar.Value;
            }
        }

        private void numericVLimitLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                editor.Vincent.LimitLevel = (byte)numericVLimitLevel.Value;
            }
        }

        //allow all weapons
        private void checkBoxCSAllowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.CaitSith != null)
            {
                comboBoxCSWeapon.SuspendLayout();
                comboBoxCSWeapon.Items.Clear();
                if (checkBoxCSAllowAll.Checked) //all weapons
                {
                    foreach (var w in GameData.WEAPON_LIST)
                    {
                        comboBoxCSWeapon.Items.Add(w.Name);
                    }
                    if (editor.CaitSith.Weapon != null)
                    {
                        comboBoxCSWeapon.SelectedIndex = GameData.WEAPON_LIST.IndexOf(editor.CaitSith.Weapon);
                    }
                }
                else //Cait weapons only
                {
                    foreach (var w in csWeaponList)
                    {
                        comboBoxCSWeapon.Items.Add(w.Name);
                    }
                    if (editor.CaitSith.Weapon != null)
                    {
                        int index = csWeaponList.IndexOf(editor.CaitSith.Weapon);
                        if (index == -1) //default to init equip
                        {
                            comboBoxCSWeapon.SelectedIndex = 0;
                            editor.CaitSith.Weapon = csWeaponList[0];
                        }
                        else
                        {
                            comboBoxCSWeapon.SelectedIndex = index;
                        }
                    }
                }
                comboBoxCSWeapon.ResumeLayout();
            }
        }

        private void checkBoxVAllowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null && editor.Vincent != null)
            {
                comboBoxVWeapon.SuspendLayout();
                comboBoxVWeapon.Items.Clear();
                if (checkBoxVAllowAll.Checked) //all weapons
                {
                    foreach (var w in GameData.WEAPON_LIST)
                    {
                        comboBoxVWeapon.Items.Add(w.Name);
                    }
                    if (editor.Vincent.Weapon != null)
                    {
                        comboBoxVWeapon.SelectedIndex = GameData.WEAPON_LIST.IndexOf(editor.Vincent.Weapon);
                    }
                }
                else //Vincent weapons only
                {
                    foreach (var w in vWeaponList)
                    {
                        comboBoxVWeapon.Items.Add(w.Name);
                    }
                    if (editor.Vincent.Weapon != null)
                    {
                        int index = vWeaponList.IndexOf(editor.Vincent.Weapon);
                        if (index == -1) //default to init equip
                        {
                            comboBoxVWeapon.SelectedIndex = 0;
                            editor.Vincent.Weapon = vWeaponList[0];
                        }
                        else
                        {
                            comboBoxVWeapon.SelectedIndex = index;
                        }
                    }
                }
                comboBoxVWeapon.ResumeLayout();
            }
        }

        //change weapon
        private void comboBoxCSWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            int w = comboBoxCSWeapon.SelectedIndex;
            if (w >= 0 && editor != null && editor.CaitSith != null)
            {
                if (checkBoxCSAllowAll.Checked) //all weapons
                {
                    if (w < GameData.WEAPON_COUNT)
                    {
                        editor.CaitSith.Weapon = GameData.WEAPON_LIST[w];
                        UpdateMateriaSlots(Characters.CaitSith);
                    }
                }
                else //just Cait weapons
                {
                    if (w < csWeaponList.Count)
                    {
                        editor.CaitSith.Weapon = csWeaponList[w];
                        UpdateMateriaSlots(Characters.CaitSith);
                    }
                }
            }
        }

        private void comboBoxVWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            int w = comboBoxVWeapon.SelectedIndex;
            if (w >= 0 && editor != null && editor.Vincent != null)
            {
                if (checkBoxVAllowAll.Checked) //all weapons
                {
                    if (w < GameData.WEAPON_COUNT)
                    {
                        editor.Vincent.Weapon = GameData.WEAPON_LIST[w];
                        UpdateMateriaSlots(Characters.Vincent);
                    }
                }
                else //just Vincent weapons
                {
                    if (w < csWeaponList.Count)
                    {
                        editor.Vincent.Weapon = vWeaponList[w];
                        UpdateMateriaSlots(Characters.Vincent);
                    }
                }
            }
        }

        //change armor
        private void comboBoxCSArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxCSArmor.SelectedIndex;
            if (a >= 0 && a < GameData.ARMOR_LIST.Count && editor != null && editor.CaitSith != null)
            {
                editor.CaitSith.Armor = GameData.ARMOR_LIST[comboBoxCSArmor.SelectedIndex];
                UpdateMateriaSlots(Characters.CaitSith);
            }
        }

        private void comboBoxVArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxVArmor.SelectedIndex;
            if (a >= 0 && a < GameData.ARMOR_LIST.Count && editor != null && editor.Vincent != null)
            {
                editor.Vincent.Armor = GameData.ARMOR_LIST[comboBoxVArmor.SelectedIndex];
                UpdateMateriaSlots(Characters.Vincent);
            }
        }

        //change accesssory
        private void comboBoxCSAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxCSAccessory.SelectedIndex;
            if (editor != null && editor.CaitSith != null)
            {
                if (a > 0 && a < GameData.ACCESSORY_LIST.Count)
                {
                    editor.CaitSith.Accessory = arrangedAccessoryList[comboBoxCSAccessory.SelectedIndex - 1];
                }
                else
                {
                    editor.CaitSith.Accessory = null;
                }
            }
        }

        private void comboBoxVAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBoxVAccessory.SelectedIndex;
            if (editor != null && editor.Vincent != null)
            {
                if (a > 0 && a < GameData.ACCESSORY_LIST.Count)
                {
                    editor.Vincent.Accessory = arrangedAccessoryList[comboBoxVAccessory.SelectedIndex - 1];
                }
                else
                {
                    editor.Vincent.Accessory = null;
                }
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
            if (!stopper && editor != null && editor.CaitSith != null)
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
            if (!stopper && editor != null && editor.Vincent != null)
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

        private void listBoxItemPrices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBoxItemPrices.SelectedIndex;
            if (i != -1 && editor != null)
            {
                stopper = true;
                numericItemPrice.Enabled = true;
                var item = GetItemData(i);
                if (item is WeaponData)
                {
                    numericItemPrice.Value = editor.WeaponPrices[item.HexValue];
                }
                else if (item is ArmorData)
                {
                    numericItemPrice.Value = editor.ArmorPrices[item.HexValue];
                }
                else if (item is AccessoryData)
                {
                    numericItemPrice.Value = editor.AccessoryPrices[item.HexValue];
                }
                else
                {
                    numericItemPrice.Value = editor.ItemPrices[i];
                }
                stopper = false;
            }
        }

        private void numericItemPrice_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null)
            {
                int i = listBoxItemPrices.SelectedIndex;
                var item = GetItemData(i);
                if (item is WeaponData)
                {
                    editor.WeaponPrices[item.HexValue] = (uint)numericItemPrice.Value;
                    listBoxItemPrices.Items[i] = $"{item.Name} - {editor.WeaponPrices[item.HexValue]}";
                }
                else if (item is ArmorData)
                {
                    editor.ArmorPrices[item.HexValue] = (uint)numericItemPrice.Value;
                    listBoxItemPrices.Items[i] = $"{item.Name} - {editor.ArmorPrices[item.HexValue]}";
                }
                else if (item is AccessoryData)
                {
                    editor.AccessoryPrices[item.HexValue] = (uint)numericItemPrice.Value;
                    listBoxItemPrices.Items[i] = $"{item.Name} - {editor.AccessoryPrices[item.HexValue]}";
                }
                else
                {
                    editor.ItemPrices[i] = (uint)numericItemPrice.Value;
                    listBoxItemPrices.Items[i] = $"{item.Name} - {editor.ItemPrices[i]}";
                }
            }
        }

        private void listBoxMateriaPrices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBoxMateriaPrices.SelectedIndex;
            if (i != -1 && editor != null)
            {
                stopper = true;
                numericMateriaPrice.Enabled = true;
                int pos = GameData.MATERIA_LIST.IndexOf(arrangedMateriaList[i]);
                numericMateriaPrice.Value = editor.MateriaPrices[pos];
                stopper = false;
            }
        }

        private void numericMateriaPrice_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null)
            {
                int i = listBoxMateriaPrices.SelectedIndex,
                    pos = GameData.MATERIA_LIST.IndexOf(arrangedMateriaList[i]);
                editor.MateriaPrices[pos] = (uint)numericMateriaPrice.Value;
                listBoxMateriaPrices.Items[i] = $"{arrangedMateriaList[i].Name} - {editor.MateriaPrices[pos]}";
            }
        }

        private int GetItemIndex(ItemData item)
        {
            if (item is WeaponData)
            {
                return item.HexValue + GameData.ITEM_COUNT;
            }
            else if (item is ArmorData)
            {
                return item.HexValue + GameData.WEAPON_END - ITEM_OFFSET;
            }
            else if (item is AccessoryData)
            {
                var acc = item as AccessoryData;
                if (acc != null)
                {
                    return arrangedAccessoryList.IndexOf(acc) + GameData.ARMOR_END - ITEM_OFFSET;
                }
            }
            else if (item is MateriaData)
            {
                var mat = item as MateriaData;
                if (mat != null)
                {
                    return arrangedMateriaList.IndexOf(mat) + GameData.ACCESSORY_END - ITEM_OFFSET;
                }
            }
            else
            {
                return item.HexValue;
            }
            return 0;
        }

        private ItemData GetItemData(int index)
        {
            if (index < GameData.ITEM_COUNT)
            {
                return GameData.ITEM_LIST[index];
            }
            else if (index < GameData.WEAPON_END - ITEM_OFFSET)
            {
                return GameData.WEAPON_LIST[index - GameData.ITEM_COUNT];
            }
            else if (index < GameData.ARMOR_END - ITEM_OFFSET)
            {
                return GameData.ARMOR_LIST[index - GameData.WEAPON_END + ITEM_OFFSET];
            }
            else if (index < GameData.ACCESSORY_END - ITEM_OFFSET)
            {
                return arrangedAccessoryList[index - GameData.ARMOR_END + ITEM_OFFSET];
            }
            else
            {
                return arrangedMateriaList[index - GameData.ACCESSORY_END + ITEM_OFFSET];
            }
        }

        //select shop
        private void comboBoxShopIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBoxShopIndex.SelectedIndex;
            if (i != -1 && editor != null)
            {
                stopper = true;
                comboBoxShopType.SelectedIndex = (int)editor.Shops[i].ShopType;
                numericShopItemCount.Value = editor.Shops[i].ItemCount;

                for (int j = 0; j < ShopInventory.SHOP_ITEM_MAX; ++j)
                {
                    if (j < editor.Shops[i].ItemCount)
                    {
                        var item = editor.Shops[i].Inventory[j];
                        if (item != null)
                        {
                            shopItemList[j].Enabled = true;
                            shopItemList[j].SelectedIndex = GetItemIndex(item);
                        }
                        else
                        {
                            shopItemList[j].Enabled = false;
                        }
                    }
                    else
                    {
                        shopItemList[j].Enabled = false;
                    }
                }
                stopper = false;
            }
        }

        //change shop item count
        private void numericShopItemCount_ValueChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null)
            {
                //enable/disable comboboxes
                for (int i = 0; i < ShopInventory.SHOP_ITEM_MAX; ++i)
                {
                    shopItemList[i].Enabled = i < numericShopItemCount.Value;
                }

                //change shop inventory
                var shop = editor.Shops[comboBoxShopIndex.SelectedIndex];
                if (numericShopItemCount.Value > shop.ItemCount)
                {
                    var item = GetItemData(shopItemList[shop.ItemCount].SelectedIndex);
                    shop.AddItem(item);
                }
                else
                {
                    shop.RemoveItem();
                }
            }
        }

        //change shop item
        private void comboBoxShopItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!stopper && editor != null)
            {
                for (int i = 0; i < ShopInventory.SHOP_ITEM_MAX; ++i)
                {
                    if (sender == shopItemList[i]) //check which combobox sent the command
                    {
                        var shop = editor.Shops[comboBoxShopIndex.SelectedIndex];
                        var item = GetItemData(shopItemList[i].SelectedIndex);
                        shop.Inventory[i] = item;
                        break;
                    }
                }
            }
        }

        //load char data from a file
        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }

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
                        try
                        {
                            editor.ReadFile(path);
                        }
                        catch (EndOfStreamException ex)
                        {
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        UpdateFormData();
                    }
                    else
                    {
                        MessageBox.Show($"File could not be found.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"{ex.Message} ({ex.InnerException?.Message})", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //save char data to a file
        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            if (editor == null) { throw new ArgumentNullException(nameof(editor)); }

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
                    MessageBox.Show($"{ex.Message} ({ex.InnerException?.Message})", "Error",
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
                if (control == buttonHext && editor != null && editor.Language != Language.English)
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
            try
            {
                if (editor == null) { throw new ArgumentNullException(nameof(editor)); }

                DialogResult result;
                string path;
                if (vanilla == null)
                {
                    MessageBox.Show("You will need to provide an unmodified EXE for the Hext comparison.",
                        "EXE Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    using (var openDialog = new OpenFileDialog())
                    {
                        openDialog.Filter = "Final Fantasy VII executable|ff7_en.exe;ff7_es.exe;ff7_fr.exe;ff7_de.exe;ff7.exe";
                        result = openDialog.ShowDialog();
                        path = openDialog.FileName;
                    }

                    if (result != DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            vanilla = new EXEeditor(path);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

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
                        MessageBox.Show($"{ex.Message} ({ex.InnerException?.Message})", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //update EXE
        private void buttonSaveEXE_Click(object sender, EventArgs e)
        {
            try
            {
                if (editor == null) { throw new ArgumentNullException(nameof(editor)); }
                editor.WriteEXE();
                MessageBox.Show("Saved successfully!");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"{ex.Message} ({ex.InnerException?.Message})", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
