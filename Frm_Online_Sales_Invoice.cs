using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounts;
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Bill
{
    public partial class Frm_Online_Sales_Invoice : Form, Entry 
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataGrid dgv = new DataGrid();
        DataRow Dr;
        DataRow Dr1;
        DataRow Dr3;
        String Inv_Type_Mode = "";
        Int64 Code;
        TextBox Txt = null;
        TextBox Txt1 = null;
        TextBox Txt3 = null;
        String[] Queries;
        String[] t;
        Int16 PCompCode;
        Double Reb_Amount = 0;

        public Frm_Online_Sales_Invoice()
        {
            InitializeComponent();
        }

        public void Entry_Save()
        {
            try
            {
                Int32 Array_Index = 0;
                Total_Amount();
                if (TxtPartyName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Party ..!", "Gainup");
                    TxtPartyName.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtRefNo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid ReferenceNo ..!", "Gainup");
                    TxtRefNo.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtPortInv.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid Party Invoice No ..!", "Gainup");
                    TxtPortInv.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtInvoiceType.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid InvoiceType ..!", "Gainup");
                    TxtInvoiceType.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtSalesAccount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid SalesAccount ..!", "Gainup");
                    TxtSalesAccount.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtOnAccount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid OnAccount ..!", "Gainup");
                    TxtOnAccount.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                if (TxtAgentName.Text.Trim() == string.Empty)
                {
                    TxtAgentName.Tag = 0;
                }
                if (TxtAgentCom.Text.Trim() == string.Empty)
                {
                    TxtAgentCom.Text = Convert.ToString(0);
                }
                if (CmbOType.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Invalid OrderType ..!", "Gainup");
                    CmbOType.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtAddress.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Party Address ..!", "Gainup");
                    TxtAddress.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtBillAddress.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Bill Address ..!", "Gainup");
                    TxtAddress.Focus();
                    MyParent.Save_Error = true;
                    return;
                }

                if (TxtShipAddress.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Bill Address ..!", "Gainup");
                    TxtAddress.Focus();
                    MyParent.Save_Error = true;
                    return;
                }
                
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Item Details ..!", "Gainup");
                    Grid.CurrentCell = Grid["ITEM", 0];
                    Grid.Focus();
                    Grid.BeginEdit(true);
                    MyParent.Save_Error = true;
                    return;
                }
                if (Convert.ToDouble(TxtAmt.Text) == 0)
                {
                    MessageBox.Show("Invalid Qty..!", "Gainup");
                    MyParent.Save_Error = true;
                    return;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value || Grid[j, i].Value.ToString() == "")
                        {
                            if (Grid.Columns[j].Name == "QTY" || Grid.Columns[j].Name == "RATE" || Grid.Columns[j].Name == "ITEM" || Grid.Columns[j].Name == "UOMNAME" || Grid.Columns[j].Name == "ACT_RATE" || Grid.Columns[j].Name == "PONO")
                            {
                                MessageBox.Show("'" + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                                Grid.CurrentCell = Grid[j, i];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                MyParent.Save_Error = true;
                                return;
                            }
                        }
                    }
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if ((Grid["Ocn_No", i].Value) == (Grid["Ocn_No", j].Value) && (Grid["Item", i].Value) == (Grid["Item", j].Value) && Convert.ToDouble(Grid["Rate", i].Value.ToString()) == Convert.ToDouble(Grid["Rate", j].Value.ToString()) && Grid["Description", i].Value.ToString() == Grid["Description", j].Value.ToString())
                        {
                            MessageBox.Show("Already ITEM, RATE, OCN & Description is Available", "Gainup");
                            Grid["QTY", i].Value = "0.000";
                            Grid["RATE", i].Value = "0.00";
                            Grid["OCN_NO", i].Value = "";
                            Grid["Amount", i].Value = "0.00";
                            Grid["Disc_per", i].Value = "0.00";
                            i = Grid.Rows.Count;
                            j = Grid.Rows.Count;
                            MyParent.Save_Error = true;
                            Total_Amount();
                            return;
                        }
                        if (Grid["Qty", i].Value.ToString() == String.Empty || Grid["Amount", i].Value.ToString() == String.Empty)
                        {
                            Grid["Qty", i].Value = "0";
                            Grid["Rate", i].Value = "0";
                            Grid["Amount", i].Value = "0";
                            Grid["Disc_Per", i].Value = "0";
                        }
                    }
                }

                Double OQty = 0;

                for (int i = 0; i <= Grid.Rows.Count - 2; i++)
                {
                    if (Convert.ToDouble(Grid["QTY", i].Value) > 0)
                    {
                        OQty = Convert.ToDouble(MyBase.SumWithCondtion(ref Grid, "Qty", "OCN_NO", Grid["OCN_NO", i].Value.ToString()));
                        if (OQty > 0)
                        {
                            DataTable TDto = new DataTable();
                            MyBase.Load_Data("SElect ORdeR_NO, Buyer_Qty - (Case When '" + MyParent.Edit + "' = 'true' Then 0 Else " + Convert.ToDouble(Grid["Qty", i].Value.ToString()) + " End) Bal_Qty FRom Online_Sales_Order_Details_Fn() Where ORdeR_NO = '" + Grid["OCN_NO", i].Value.ToString() + "' ", ref TDto);
                            if (TDto.Rows.Count > 0)
                            {
                                if (Convert.ToDouble(TDto.Rows[0]["Bal_Qty"].ToString()) < 0)
                                {
                                    MessageBox.Show("Invalid OCN Qty,  " + TDto.Rows[0]["Bal_Qty"].ToString() + " Excess Qty ", "GAinup");
                                    Grid["QTY", i].Value = "0.00";
                                    Grid.CurrentCell = Grid["Ocn_No", i];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    MyParent.Save_Error = true;
                                    return;
                                }
                            }
                        }
                    }
                }
                
                if (MyParent._New)
                {
                    Queries = new String[Dt.Rows.Count + 10 + Dt1.Rows.Count];

                    DataTable TDt1 = new DataTable();
                    DataTable TDt2 = new DataTable();
                    DataTable TDt4 = new DataTable();
                    DataTable TDt5 = new DataTable();

                    MyBase.Load_Data("Select  Invoice_No_Prefix From Invoice_Type_Settings  where Company_Code = " + MyParent.CompCode + " and  Invoice_Type_Rowid = " + TxtInvoiceType.Tag + " ", ref TDt1);

                    MyBase.Load_Data("Select (Isnull(Max(Cast(SubString(Invoice_No, 2 , 4) as int)), 0) + 1) No, '/' + Substring('" + MyParent.YearCode + "' ,3,2)  + '-' + Substring('" + MyParent.YearCode + "', 8,2) YCode  From S_Sales_Invoice_Master  where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and Invoice_No like '%" + (TDt1.Rows[0][0]) + "%' and Invoice_Date >= '01-jul-2017' ", ref TDt2);
                    MyBase.Load_Data("Select (Isnull(Max(Cast(SubString(Invoice_No, 2 , 4) as int)), 0) + 1) No, '/' + Substring('" + MyParent.YearCode + "' ,3,2)  + '-' + Substring('" + MyParent.YearCode + "', 8,2) YCode  From W_Sales_Invoice_Master  where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and Invoice_No like '%" + (TDt1.Rows[0][0]) + "%' and Invoice_Date >= '01-jul-2017' ", ref TDt4);
                    MyBase.Load_Data("Select (Isnull(Max(Cast(SubString(Invoice_No, 3 , 4) as int)), 0) + 1) No, '/' + Substring('" + MyParent.YearCode + "' ,3,2)  + '-' + Substring('" + MyParent.YearCode + "', 8,2) YCode  From S_Online_Sales_Order_Master where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and Invoice_No like '%" + (TDt1.Rows[0][0]) + "%' and Invoice_Date >= '01-jul-2017' ", ref TDt5);
                    if (Convert.ToDouble(TDt2.Rows[0][0]) > Convert.ToDouble(TDt4.Rows[0][0]) && Convert.ToDouble(TDt2.Rows[0][0]) > Convert.ToDouble(TDt5.Rows[0][0]))
                    {
                        TxtInvoiceNo.Text = Convert.ToString(TDt1.Rows[0][0]) + String.Format("{0:0000}", Convert.ToDouble(TDt2.Rows[0][0])) + TDt2.Rows[0][1].ToString();
                    }
                    else if (Convert.ToDouble(TDt4.Rows[0][0]) > Convert.ToDouble(TDt5.Rows[0][0]))
                    {
                        TxtInvoiceNo.Text = Convert.ToString(TDt1.Rows[0][0]) + String.Format("{0:0000}", Convert.ToDouble(TDt4.Rows[0][0])) + TDt4.Rows[0][1].ToString();
                    }
                    else
                    {
                        TxtInvoiceNo.Text = Convert.ToString(TDt1.Rows[0][0]) + String.Format("{0:0000}", Convert.ToDouble(TDt5.Rows[0][0])) + TDt5.Rows[0][1].ToString();
                    }

                    Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Master (Invoice_No, Invoice_date, Party_Code, Order_No, Order_Date, Invoice_Type_RowID, Party_Invoice_No, Party_Invoice_Date, Sales_Code, OnAcCode, Agent_Code, Agent_Per, Order_Type, Gross_Amount, Other_Charges, Net_Amount, Party_Address, Company_Code, Year_Code, Approval_Status, Approval_Timing, Ro_Amount, TNet_Amount, Charges, Tax_Amount, Bill_Address, Ship_Address, User_Code, Entry_System, Entry_Time, Remarks) Values ('" + TxtInvoiceNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtPartyName.Tag.ToString() + ", '" + TxtRefNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', " + TxtInvoiceType.Tag + ", '" + TxtPortInv.Text.ToString() + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpPortInvoiceDate.Value) + "', " + TxtSalesAccount.Tag.ToString() + ", " + TxtOnAccount.Tag.ToString() + ", " + TxtAgentName.Tag.ToString() + ", " + TxtAgentCom.Text.ToString() + ", '" + CmbOType.Text.ToString() + "', " + Convert.ToDouble(TxtAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtChargesAmount.Text.ToString()) + ", " + Convert.ToDouble(TxtNetAmt.Text.ToString()) + ", '" + TxtAddress.Text.ToString() + "', " + MyParent.CompCode + ", '" + MyParent.YearCode + "', 'T', Getdate(), " + Convert.ToDouble(TxtRoAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtNetAmt.Text.ToString()) + ", " + Convert.ToDouble(TxtChargesAmount.Text.ToString()) + ", " + Convert.ToDouble(TxtGrossAmt.Text.ToString()) + ", '" + TxtBillAddress.Text.ToString() + "', '" + TxtShipAddress.Text.ToString() + "', " + MyParent.UserCode + ", Host_Name(), Getdate(), '" + TxtRemarks.Text.ToString() + "') ; Select Scope_Identity()";
                    Queries[Array_Index++] = MyParent.EntryLog("SOCKS ONLINE INVOICE", "ADD", "@@IDENTITY");
                }
                else
                {
                    Queries = new String[Dt.Rows.Count + 10 + Dt1.Rows.Count];
                    Queries[Array_Index++] = "Update S_Online_Sales_Order_Master Set Party_Code = " + TxtPartyName.Tag.ToString() + ", Order_No = '" + TxtRefNo.Text + "', Order_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpODate.Value) + "', Invoice_Type_RowID = " + TxtInvoiceType.Tag.ToString() + ", Party_Invoice_No = '" + TxtPortInv.Text.ToString() + "', Party_Invoice_Date = '" + String.Format("{0:dd-MMM-yyyy}", DtpPortInvoiceDate.Value) + "', Sales_Code = " + TxtSalesAccount.Tag.ToString() + ", OnAcCode = " + TxtOnAccount.Tag.ToString() + ", Agent_Code = " + TxtAgentName.Tag.ToString() + ", Agent_Per = " + TxtAgentCom.Text + ", Order_Type = '" + CmbOType.Text + "', Gross_Amount = " + Convert.ToDouble(TxtAmt.Text) + ", Other_Charges = " + Convert.ToDouble(TxtChargesAmount.Text) + ", Net_Amount = " + Convert.ToDouble(TxtNetAmt.Text) + ", Party_Address = '" + TxtAddress.Text.ToString() + "', Company_Code = " + MyParent.CompCode + ", Ro_Amount =" + Convert.ToDouble(TxtRoAmt.Text) + ", TNet_Amount =" + Convert.ToDouble(TxtNetAmt.Text) + ", Charges =  " + Convert.ToDouble(TxtChargesAmount.Text) + ", Tax_Amount =  " + Convert.ToDouble(TxtGrossAmt.Text.ToString()) + ", Bill_Address = '" + TxtBillAddress.Text.ToString() + "', Ship_Address = '" + TxtShipAddress.Text.ToString() + "', User_Code = " + MyParent.UserCode + ", Entry_System = Host_Name(), Entry_Time = Getdate(), Remarks = '" + TxtRemarks.Text.ToString() + "' Where SoCode = " + Code;
                    Queries[Array_Index++] = "Delete From S_Online_Sales_Order_Details Where SoCode = " + Code;
                    Queries[Array_Index++] = "Delete From S_Online_Sales_Order_Tax Where SoCode = " + Code;
                    Queries[Array_Index++] = "Delete From S_Online_Sales_Order_Charges Where SoCode = " + Code;
                    Queries[Array_Index++] = MyParent.EntryLog("SOCKS ONLINE INVOICE", "EDIT", Code.ToString());
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Details (SoCode, Slno, Ocn_No, Item_ID, Description, HSN, Uom_ID, Qty, Rate, Disc_Per, Disc_Amt, Amount, Company_Code, Year_Code)Values (@@IDENTITY, " + (i + 1) + ", '" + Grid["Ocn_No", i].Value + "', " + Grid["Item_ID", i].Value + ", '" + Grid["Description", i].Value + "', '" + Grid["HSN", i].Value + "', " + Grid["UOM_Id", i].Value + ", " + Grid["Qty", i].Value + ", " + Grid["Rate", i].Value + ", " + Grid["Disc_Per", i].Value + ", " + Grid["Disc_Amt", i].Value + ", " + Grid["AMOUNT", i].Value + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Details (SoCode, Slno, Ocn_No, Item_ID, Description, HSN, Uom_ID, Qty, Rate, Disc_Per, Disc_Amt, Amount, Company_Code, Year_Code)Values (" + Code + ", " + (i + 1) + ", '" + Grid["Ocn_No", i].Value + "', " + Grid["Item_ID", i].Value + ", '" + Grid["Description", i].Value + "', '" + Grid["HSN", i].Value + "', " + Grid["UOM_Id", i].Value + ", " + Grid["Qty", i].Value + ", " + Grid["Rate", i].Value + ", " + Grid["Disc_Per", i].Value + ", " + Grid["Disc_Amt", i].Value + ", " + Grid["AMOUNT", i].Value + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";
                    }
                }
                if (Dt1.Rows.Count >= 1)
                {
                    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    {
                        if (MyParent._New)
                        {
                            Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Tax (SOCode, Slno, Tax_Code, Tax_Per, Tax_Amount, Company_Code, Year_Code) Values (@@IDENTITY, " + (i + 1) + ", " + Grid1["Tax_Code", i].Value + ", " + Grid1["PERCENTAGE", i].Value + ", " + Grid1["TAXAMOUNT", i].Value + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";
                        }
                        else
                        {
                            Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Tax (SOCode, Slno, Tax_Code, Tax_Per, Tax_Amount, Company_Code, Year_Code) Values (" + Code + ", " + (i + 1) + ", " + Grid1["Tax_Code", i].Value + ", " + Grid1["PERCENTAGE", i].Value + ", " + Grid1["TAXAMOUNT", i].Value + ", " + MyParent.CompCode + ", '" + MyParent.YearCode + "')";
                        }
                    }
                }
                for (int i = 0; i <= Dt3.Rows.Count - 1; i++)
                {
                    if (MyParent._New)
                    {
                        Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Charges (SOCode, Slno, Charges_ID, Charges_Amount) Values (@@IDENTITY, " + (i + 1) + ", " + Grid3["Charges_Id", i].Value + ", " + Grid3["AMOUNT", i].Value + ")";
                    }
                    else
                    {
                        Queries[Array_Index++] = "Insert into S_Online_Sales_Order_Charges (SOCode, Slno, Charges_ID, Charges_Amount) Values (" + Code + ", " + (i + 1) + ", " + Grid3["Charges_Id", i].Value + ", " + Grid3["AMOUNT", i].Value + ")";
                    }
                }
                
                if (MyParent._New)
                {
                    MyBase.Run_Identity(false, Queries);
                }
                else
                {
                    MyBase.Run_Identity(true, Queries);
                }
                MyParent.Save_Error = false;
                MessageBox.Show("Saved ..!", "Gainup");
            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                Code = Convert.ToInt64(Dr["SOCODE"]);
                TxtInvoiceNo.Text = Dr["Invoice_No"].ToString();
                TxtInvoiceNo.Tag = Dr["SOCODE"];
                DtpDate.Value = Convert.ToDateTime(Dr["Invoice_DATE"]);
                TxtRefNo.Text = Dr["Order_No"].ToString();
                DtpODate.Value = Convert.ToDateTime(Dr["Order_Date"]);
                TxtPortInv.Text = Dr["Party_Invoice_No"].ToString();
                DtpPortInvoiceDate.Value = Convert.ToDateTime(Dr["Party_Invoice_DATE"]);
                TxtPartyName.Tag = Dr["PARTY_CODE"].ToString();
                TxtPartyName.Text = Dr["PARTY"].ToString();
                TxtInvoiceType.Text = Dr["INVOICE_TYPE"].ToString();
                TxtInvoiceType.Tag = Dr["INVOICE_TYPE_RowID"].ToString();
                Inv_Type_Mode = Dr["INVOICE_TYPE"].ToString();
                TxtSalesAccount.Text = Dr["SALES_ACCOUNT"].ToString();
                TxtSalesAccount.Tag = Dr["SALES_CODE"].ToString();
                TxtOnAccount.Text = Dr["ONACCOUNT"].ToString();
                TxtOnAccount.Tag = Dr["OnAcCode"].ToString();
                TxtAgentName.Text = Dr["AGENT"].ToString();
                TxtAgentName.Tag = Dr["AGENT_CODE"].ToString();
                CmbOType.Text = Dr["ORDER_TYPE"].ToString();
                TxtAgentCom.Text = Dr["AGENT_Per"].ToString();
                TxtAddress.Text = Dr["Delivery_ADDRESS"].ToString();
                TxtBillAddress.Text = Dr["Bill_ADDRESS"].ToString();
                TxtShipAddress.Text = Dr["Ship_ADDRESS"].ToString();
                Grid_Data();
                TxtAmt.Text = Dr["GROSS_AMOUNT"].ToString();
                TxtGrossAmt.Text = Dr["Ord_Tax_Amt"].ToString();
                TxtRoAmt.Text = Dr["RO_AMOUNT"].ToString();
                TxtNetAmt.Text = Dr["TNET_AMOUNT"].ToString();
                TxtRemarks.Text = Dr["Remarks"].ToString();
                Total_Amount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                listBox1.Items.Clear();
                Grid_Data();
                CmbOType.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);
                listBox1.Items.Clear();
                String Str = " Select Distinct Invoice_no,Invoice_Date, Order_No, Order_Date, Party, Party_Invoice_No, Party_Invoice_Date, Item, In_Qty Qty, Rate, OCn_NO, Amount, Order_Type, Sales_Account, ";
                Str = Str + " In_Other_Amt Other_Amount, Ord_Tax_Amt, In_Net_Amt, Delivery_Address, Bill_address, Ship_Address, In_Ro_Amt Ro_Amount, In_TNet_Amt TNet_Amount,in_Gross_Amt Gross_Amount, ";
                Str = Str + " in_Master_Id, Party_Code, Invoice_Type_RowID, Sales_Code, OnAccount,Invoice_Type, OnAcCode, Agent, Agent_Code, Agent_Per, Remarks, SoCode ";
                Str = Str + " From S_Online_Sales_Invoice_Fn_WoTax(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where BillPassing_Status = 0 Order by In_Master_Id  Desc ";
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Online Sales Invoice - Edit", Str, String.Empty, 100, 100, 100, 100, 200, 150, 150, 100, 100, 100, 120, 100, 120, 120);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    TxtPartyName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                listBox1.Items.Clear();
                String Str = "Select Distinct Invoice_no,Invoice_Date, Order_No, Order_Date, Party, Party_Invoice_No, Party_Invoice_Date, Item, In_Qty Qty, Rate, OCn_NO, Amount, Order_Type, Sales_Account, ";
                Str = Str + " In_Other_Amt Other_Amount, Ord_Tax_Amt, In_Net_Amt, Delivery_Address, Bill_address, Ship_Address, In_Ro_Amt Ro_Amount, In_TNet_Amt TNet_Amount,in_Gross_Amt Gross_Amount, BillPassing_Status, ";
                Str = Str + " in_Master_Id, Party_Code, Invoice_Type_RowID, Sales_Code, OnAccount,Invoice_Type, OnAcCode, Agent, Agent_Code, Agent_Per, Remarks, SoCode ";
                Str = Str + " From S_Online_Sales_Invoice_Fn_WoTax(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Order by In_Master_Id  Desc ";
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Online Sales - View", Str, String.Empty, 100, 100, 100, 100, 200, 150, 150, 100, 100, 100, 120, 100, 120, 120, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                listBox1.Items.Clear();
                String Str = " Select Distinct Invoice_no,Invoice_Date, Order_No, Order_Date, Party, Party_Invoice_No, Party_Invoice_Date, Item, In_Qty Qty, Rate, OCn_NO, Amount, Order_Type, Sales_Account, ";
                Str = Str + " In_Other_Amt Other_Amount, Ord_Tax_Amt, In_Net_Amt, Delivery_Address, Bill_address, Ship_Address, In_Ro_Amt Ro_Amount, In_TNet_Amt TNet_Amount,in_Gross_Amt Gross_Amount, ";
                Str = Str + " in_Master_Id, Party_Code, Invoice_Type_RowID, Sales_Code, OnAccount,Invoice_Type, OnAcCode, Agent, Agent_Code, Agent_Per, Remarks, SoCode ";
                Str = Str + " From S_Online_Sales_Invoice_Fn_WoTax(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') Where BillPassing_Status = 0 Order by In_Master_Id  Desc ";
                Dr = Tool.Selection_Tool_Resize(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Online Sales Invoice - Delete", Str, String.Empty, 100, 100, 100, 100, 200, 150, 150, 100, 100, 100, 120, 100, 120, 120);
                if (Dr != null)
                {
                    Fill_Datas(Dr);
                    MyParent.Load_DeleteConfirmEntry();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Code > 0)
                {
                    MyBase.Run("Delete From S_Online_Sales_Order_Charges Where SoCode = " + Code, "Delete from S_Online_Sales_Order_Tax where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and SoCode = " + Code, "Delete from S_Online_Sales_Order_Details where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and SoCode = " + Code, "Delete From S_Online_Sales_Order_Master where Company_Code = " + MyParent.CompCode + " and  Year_Code ='" + MyParent.YearCode + "' and SoCode = " + Code, MyParent.EntryLog("SOCKS Sales Invoice", "DELETE", Code.ToString()));
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyBase.Clear(this);
                }
                MyParent.Load_DeleteEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Print()
        {
            try
            {
                GBReport.Visible = true;
                RBGstPrint.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Online_Sales_Invoice_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                Disable();
                //MyBase.Disable_Cut_Copy(GBMain);
                if (MyParent.CompCode == 1)
                {
                    PCompCode = 1;
                }
                else if (MyParent.CompCode == 2)
                {
                    PCompCode = 3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Disable()
        {
            try
            {
                foreach (Control Ct in GBMain.Controls)
                {
                    if (Ct is System.Windows.Forms.TextBox)
                    {
                        if (Ct.Name != TxtAddress.Name && Ct.Name != TxtBillAddress.Name && Ct.Name != TxtShipAddress.Name && Ct.Name != TxtRefNo.Name && Ct.Name != TxtPortInv.Name)
                        {
                            Ct.ContextMenu = new ContextMenu();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Data()
        {
            String Str = String.Empty;
            String Str1 = String.Empty;
            String Str3 = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    Str = " Select S.SlNo as SNO, S.Ocn_No, I.Name ITEM, S.Description, S.HSN, U.Name UOM, S.Qty, S.Qty Conv_Qty, S.Qty Qty2, S.Rate, S.Disc_Per, S.Disc_Amt, S.Amount, S.SoCode, S.Item_ID, S.Uom_ID, S.RowID From [S_Online_Sales_Order_Details] S ";
                    Str = Str + " Left Join Socks_Item_Master I On S.Item_ID = I.RowID Left Join UOM_Master U on S.UOM_ID = U.RowID Where 1 = 2 ";
                    Str1 = "Select  S.SlNo as SNO, b.Ledger_Name as TAXACCOUNT, S.tax_per PERCENTAGE, 0.00 TAXAMOUNT, S.Tax_Code, S.ROWID from S_Online_Sales_Order_Tax S Left join Invoice_Type_TaxHead a  on S.Tax_Code = a.Tax_Code left join Accounts.Dbo.Tax_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') b on a.Tax_Code =b.Ledger_Code  where 1=2";
                    Str3 = "Select S.Slno SNO, s1.name CHARGES_NAME, s.charges_amount AMOUNT, s.charges_id  From S_Online_Sales_Order_Charges S Left Join Charges_Master S1 on S.Charges_id = S1.Rowid  Where 1 = 2";
                }
                else
                {
                    Str = " Select S.SlNo as SNO, S.Ocn_No, I.Name ITEM, S.Description, S.HSN, U.Name UOM, S.Qty, S.Qty Conv_Qty, S.Qty Qty2, S.Rate, S.Disc_Per, S.Disc_Amt, S.Amount, S.SoCode, S.Item_ID, S.Uom_ID, S.RowID From S_Online_Sales_Order_Details S ";
                    Str = Str + " Left Join Socks_Item_Master I On S.Item_ID = I.RowID Left Join UOM_Master U on S.UOM_ID = U.RowID Where S.Company_Code = " + MyParent.CompCode + "  and S.SoCode = " + Code + " Order by  S.SlNo ";
                    Str1 = "Select Distinct S.Slno as SNO, b.Ledger_Name as TAXACCOUNT, S.tax_per PERCENTAGE, S.TAX_AMOUNT TAXAMOUNT, S.Tax_Code, S.ROWID from S_Online_Sales_Order_Tax S Left join Invoice_Type_TaxHead a on S.Tax_Code = a.Tax_Code left join Accounts.Dbo.Tax_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') b on a.Tax_Code =b.Ledger_Code  where S.Company_Code = " + MyParent.CompCode + " and  a.Company_Code = " + MyParent.CompCode + " and  S.SoCode = " + Code + " Order by s.slno ";
                    Str3 = "Select Distinct S.Slno SNO, s1.name CHARGES_NAME, s.charges_amount AMOUNT, s.charges_id  From S_Online_Sales_Order_Charges S Left Join Charges_Master S1 on S.Charges_id = S1.Rowid  Where S.SOCode = " + Code;
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                Grid1.DataSource = MyBase.Load_Data(Str1, ref Dt1);
                Grid3.DataSource = MyBase.Load_Data(Str3, ref Dt3);
                MyBase.Grid_Designing(ref Grid, ref Dt, "SOCODE", "ROWID", "Item_ID", "UOM_ID", "Conv_Qty", "Qty2");
                MyBase.Grid_Designing(ref Grid1, ref Dt1, "Tax_Code", "ROWID");
                MyBase.Grid_Designing(ref Grid3, ref Dt3, "Charges_Id");
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref Grid1, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Colouring(ref Grid3, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.ReadOnly_Grid(ref Grid, "SNO", "AMOUNT", "DISC_AMT");
                MyBase.ReadOnly_Grid(ref Grid1, "SNO", "PERCENTAGE", "TAXAMOUNT");
                MyBase.ReadOnly_Grid(ref Grid3, "SNO");
                MyBase.Grid_Width(ref Grid, 50, 120, 150, 200, 100, 100, 100, 100, 120, 100, 100, 120, 100);
                MyBase.Grid_Width(ref Grid1, 50, 150, 150, 120);
                MyBase.Grid_Width(ref Grid3, 50, 150, 150);
                Grid.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["Ocn_No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Grid.Columns["ITEM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["UOM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["DISC_PER"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["DISC_AMT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid.Columns["AMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid1.Columns["TAXACCOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid1.Columns["PERCENTAGE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid1.Columns["TAXAMOUNT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Grid3.Columns["SNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid3.Columns["CHARGES_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Grid.Columns["RATE"].DefaultCellStyle.Format = "0.000000";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Frm_Online_Sales_Invoice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == TxtBillAddress.Name)
                    {
                        Grid.CurrentCell = Grid["Ocn_No", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtTotQty")
                    {
                        Grid1.CurrentCell = Grid1["TAXACCOUNT", 0];
                        Grid1.Focus();
                        Grid1.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == TxtAgentCom.Name)
                    {
                        TxtAddress.Focus();
                        SendKeys.Send("{END}");
                        return;
                    }
                    else if (this.ActiveControl.Name == TxtNetAmt.Name)
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                            return;
                        }
                    }
                    if (this.ActiveControl.Name != TxtAddress.Name)
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == TxtPartyName.Name && Grid.Rows.Count <= 1)
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select PartyName", "Select Party,Code,Address, GST_No From Accounts.DBO.Debtors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "') ", String.Empty, 400);
                        
                        if (Dr != null)
                        {
                            TxtPartyName.Text = Dr["Party"].ToString();
                            TxtPartyName.Tag = Dr["Code"].ToString();
                            TxtOnAccount.Text = Dr["Party"].ToString();
                            TxtOnAccount.Tag = Dr["Code"].ToString();
                            TxtAddress.Text = Dr["Address"].ToString();
                            TxtAddress.Tag = Dr["GST_No"].ToString();
                            // TxtRefNo.Text = "";
                        }
                    }
                    else if (this.ActiveControl.Name == TxtInvoiceType.Name && Grid1.Rows.Count <= 1)
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select InvoiceType", "Select Distinct Name, c.Ledger_Name SALESACCOUNT,  a.RowID, b.Sales_Ac_Code, a.Mode   From Invoice_Type a Left Join Invoice_Type_SalesHead b on a.RowID = b.RowID   left join Accounts.Dbo.Sales_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') c on b.Sales_Ac_Code =c.Ledger_Code Where b.Company_Code = " + MyParent.CompCode + " and b.Rowid is not null and A.Rowid in (18,19,20,21,35,38,36) Order by a.RowID ", String.Empty, 200, 200);
                        if (Dr != null)
                        {
                            TxtInvoiceType.Text = Dr["Name"].ToString();
                            TxtInvoiceType.Tag = Dr["Rowid"].ToString();
                            TxtSalesAccount.Text = Dr["SALESACCOUNT"].ToString();
                            TxtSalesAccount.Tag = Dr["Sales_Ac_Code"].ToString();
                            Inv_Type_Mode = Dr["Mode"].ToString();

                            DataTable TDt1 = new DataTable();
                            DataTable TDt2 = new DataTable();
                            DataTable TDt4 = new DataTable();
                            DataTable TDt5 = new DataTable();

                            MyBase.Load_Data("Select 'EC' Invoice_No_Prefix From Invoice_Type_Settings  where Company_Code = " + MyParent.CompCode + " and  Invoice_Type_Rowid = " + TxtInvoiceType.Tag + " ", ref TDt1);

                            MyBase.Load_Data("Select (Isnull(Max(Cast(SubString(Invoice_No, 2 , 4) as int)), 0) + 1) No, '/' + Substring('" + MyParent.YearCode + "' ,3,2)  + '-' + Substring('" + MyParent.YearCode + "', 8,2) YCode  From S_Sales_Invoice_Master  where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and Invoice_No like '%" + (TDt1.Rows[0][0]) + "%' and Invoice_Date >= '01-jul-2017' ", ref TDt2);
                            MyBase.Load_Data("Select (Isnull(Max(Cast(SubString(Invoice_No, 2 , 4) as int)), 0) + 1) No, '/' + Substring('" + MyParent.YearCode + "' ,3,2)  + '-' + Substring('" + MyParent.YearCode + "', 8,2) YCode  From W_Sales_Invoice_Master  where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and Invoice_No like '%" + (TDt1.Rows[0][0]) + "%' and Invoice_Date >= '01-jul-2017' ", ref TDt4);
                            MyBase.Load_Data("Select (Isnull(Max(Cast(SubString(Invoice_No, 3 , 4) as int)), 0) + 1) No, '/' + Substring('" + MyParent.YearCode + "' ,3,2)  + '-' + Substring('" + MyParent.YearCode + "', 8,2) YCode  From S_Online_Sales_Order_Master where Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "' and Invoice_No like '%" + (TDt1.Rows[0][0]) + "%' and Invoice_Date >= '01-jul-2017' ", ref TDt5);
                            if (Convert.ToDouble(TDt2.Rows[0][0]) > Convert.ToDouble(TDt4.Rows[0][0]) && Convert.ToDouble(TDt2.Rows[0][0]) > Convert.ToDouble(TDt5.Rows[0][0]))
                            {
                                TxtInvoiceNo.Text = Convert.ToString(TDt1.Rows[0][0]) + String.Format("{0:0000}", Convert.ToDouble(TDt2.Rows[0][0])) + TDt2.Rows[0][1].ToString();
                            }
                            else if (Convert.ToDouble(TDt4.Rows[0][0]) > Convert.ToDouble(TDt5.Rows[0][0]))
                            {
                                TxtInvoiceNo.Text = Convert.ToString(TDt1.Rows[0][0]) + String.Format("{0:0000}", Convert.ToDouble(TDt4.Rows[0][0])) + TDt4.Rows[0][1].ToString();
                            }
                            else
                            {
                                TxtInvoiceNo.Text = Convert.ToString(TDt1.Rows[0][0]) + String.Format("{0:0000}", Convert.ToDouble(TDt5.Rows[0][0])) + TDt5.Rows[0][1].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == TxtSalesAccount.Name && Grid1.Rows.Count <= 1)
                    {
                        if (TxtInvoiceType.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("Invalid InvoiceType ..!", "Gainup");
                            TxtInvoiceType.Focus();
                            return;
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Sales Account", "Select  b.Ledger_Name as SALESACCOUNT,a.Sales_Ac_Code ,a.RowID  from Invoice_Type_SalesHead a left join Accounts.Dbo.Sales_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') b on a.Sales_Ac_Code =b.Ledger_Code  where a.Company_Code = " + MyParent.CompCode + " and  Rowid = " + TxtInvoiceType.Tag.ToString() + " ", String.Empty, 400);
                            if (Dr != null)
                            {
                                TxtSalesAccount.Text = Dr["SALESACCOUNT"].ToString();
                                TxtSalesAccount.Tag = Dr["Sales_Ac_Code"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == TxtOnAccount.Name)
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select OnAccount", "Select Party,Code,'' Address From Accounts.DBO.Branch_Division (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 400);
                        
                        if (Dr != null)
                        {
                            TxtOnAccount.Text = Dr["Party"].ToString();
                            TxtOnAccount.Tag = Dr["Code"].ToString();
                            TxtAddress.Text = Dr["Address"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == TxtAgentName.Name)
                    {
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "Select Agent", "Select Party,Code From Accounts.DBO.Creditors (" + MyParent.CompCode + ", '" + MyParent.YearCode + "')", String.Empty, 400);
                        if (Dr != null)
                        {
                            TxtAgentName.Text = Dr["Party"].ToString();
                            TxtAgentName.Tag = Dr["Code"].ToString();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (this.ActiveControl.Name != TxtAddress.Name && this.ActiveControl.Name != TxtShipAddress.Name && this.ActiveControl.Name != TxtBillAddress.Name)
                    {
                        MyBase.ActiveForm_Close(this, MyParent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.TextChanged += new EventHandler(Txt_TextChanged);
                    Txt.Leave += new EventHandler(Txt_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt1 == null)
                {
                    Txt1 = (TextBox)e.Control;
                    Txt1.KeyDown += new KeyEventHandler(Txt1_KeyDown);
                    Txt1.KeyPress += new KeyPressEventHandler(Txt1_KeyPress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Ocn_No"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "OCN", "Select Order_No From Online_Sales_Order_Details_Fn() Order By Order_No", string.Empty, 200);
                        if (Dr != null)
                        {
                            Grid["OCn_No", Grid.CurrentCell.RowIndex].Value = Dr["Order_No"].ToString();
                            Txt.Text = Dr["Order_No"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["ITEM"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "ITEM", "Select Name Item, RowID from Socks_Item_Master", string.Empty, 200);
                        if (Dr != null)
                        {
                            Grid["ITEM", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            Grid["Item_ID", Grid.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                            Txt.Text = Dr["Item"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["UOM"].Index)
                    {
                        if (Grid["ITEM", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                        {
                            MessageBox.Show("Invalid Item ..!", "Gainup");
                            Grid.CurrentCell = Grid["ITEM", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        Dr = Tool.Selection_Tool(this, 50, 50, SelectionTool_Class.ViewType.NormalView, "UOM NAME", "Select Name UOM,Rowid UOMId from UOM_Master", string.Empty, 200);
                        if (Dr != null)
                        {
                            Grid["UOM", Grid.CurrentCell.RowIndex].Value = Dr["UOM"].ToString();
                            Grid["UOM_ID", Grid.CurrentCell.RowIndex].Value = Dr["UOMId"].ToString();
                            Txt.Text = Dr["UOM"].ToString();
                        }
                        Total_Amount();
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
                Double OQty = 0;
                //if (MyParent._New == true)
                //{                    
                //if (Grid["WCODE", Grid.CurrentCell.RowIndex].Value != DBNull.Value && Grid["ACT_RATE", Grid.CurrentCell.RowIndex].Value != DBNull.Value)
                //{
                //    if (Grid.Rows.Count > 2)
                //    {
                //        for (int k = 0; k < Grid.Rows.Count - 2; k++)
                //        {
                //            if (Convert.ToDouble(Grid["WCODE", k].Value) == Convert.ToDouble(Grid["WCODE", Grid.CurrentCell.RowIndex].Value) && Convert.ToDouble(Grid["ACT_RATE", k].Value) == Convert.ToDouble(Grid["ACT_RATE", Grid.CurrentCell.RowIndex].Value) && Grid["PONO", k].Value == Grid["PONO", Grid.CurrentCell.RowIndex].Value)
                //            {
                //                MessageBox.Show("Already ITEM, RATE & PONO is Available", "Gainup");
                //                Grid["BOX", Grid.CurrentCell.RowIndex].Value = "0";
                //                Grid["QTY", Grid.CurrentCell.RowIndex].Value = "0.00";
                //                Grid["ACT_RATE", Grid.CurrentCell.RowIndex].Value = "0.000000";
                //                Grid["PONO", Grid.CurrentCell.RowIndex].Value = "";
                //                k = Grid.Rows.Count;
                //                Total_Amount();                                                                        
                //                return;
                //            }
                //        }
                //    }
                //}

                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    if (Grid["DISC_PER", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        Grid["DISC_PER", Grid.CurrentCell.RowIndex].Value = 0.00;
                    }
                }
                
                //if (Grid.CurrentCell.ColumnIndex == Grid.Columns["MRP"].Index)
                //{
                //    if (Grid["MRP", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                //    {
                //        Grid["MRP", Grid.CurrentCell.RowIndex].Value = 0.00;
                //    }
                //    if (Convert.ToDouble(Grid["MRP", Grid.CurrentCell.RowIndex].Value) > 0)
                //    {
                //        Grid["EX_DUTY", Grid.CurrentCell.RowIndex].Value = 2;
                //    }
                //    else
                //    {
                //        Grid["EX_DUTY", Grid.CurrentCell.RowIndex].Value = 0;
                //    }
                //}
                //}
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QTY"].Index)
                {
                    if (Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) > 0)
                    {
                        OQty = Convert.ToDouble(MyBase.SumWithCondtion(ref Grid, "Qty", "OCN_NO", Grid["OCN_NO", Grid.CurrentCell.RowIndex].Value.ToString()));
                        if (OQty > 0)
                        {
                            DataTable TDto = new DataTable();
                            MyBase.Load_Data("SElect ORdeR_NO, Buyer_Qty - " + Convert.ToDouble(Grid["Qty", Grid.CurrentCell.RowIndex].Value.ToString()) + " Bal_Qty FRom Online_Sales_Order_Details_Fn() Where ORdeR_NO = '" + Grid["OCN_NO", Grid.CurrentCell.RowIndex].Value.ToString() + "' ", ref TDto);
                            if (TDto.Rows.Count > 0)
                            {
                                if (Convert.ToDouble(TDto.Rows[0]["Bal_Qty"].ToString()) < 0)
                                {
                                    MessageBox.Show("Invalid OCN Qty,  " + TDto.Rows[0]["Bal_Qty"].ToString() + " Excess Qty ", "GAinup");
                                    Grid["QTY", Grid.CurrentCell.RowIndex].Value = "0.00";
                                    Grid.CurrentCell = Grid["Ocn_no", Grid.CurrentCell.RowIndex];
                                    Grid.Focus();
                                    Grid.BeginEdit(true);
                                    return;
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid1.CurrentCell.ColumnIndex == Grid1.Columns["TAXACCOUNT"].Index)
                    {
                        if (TxtInvoiceType.Text.ToString() != string.Empty)
                        {
                            if (TxtSalesAccount.Tag.ToString() != "3572")
                                //if (TxtAddress.Tag.ToString().Substring(0, 2).Contains("33"))
                            {
                                Dr1 = Tool.Selection_Tool_Except_New("TAXACCOUNT", this, 50, 50, ref Dt1, SelectionTool_Class.ViewType.NormalView, "Select Tax Account", "Select  Distinct b.Ledger_Name as TAXACCOUNT,a.Tax_Code   from Invoice_Type_TaxHead a left join Accounts.Dbo.Tax_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') b on a.Tax_Code =b.Ledger_Code Left join Tax_Settings c on a.Tax_Code = c.Tax_Code  where a.Company_Code = " + MyParent.CompCode + " and a.Rowid = " + TxtInvoiceType.Tag.ToString() + "  and c.per is not null and b.Ledger_Name  Not like '%IGST%' ", String.Empty, 400);
                            }
                            else
                            {
                                Dr1 = Tool.Selection_Tool_Except_New("TAXACCOUNT", this, 50, 50, ref Dt1, SelectionTool_Class.ViewType.NormalView, "Select Tax Account", "Select  Distinct b.Ledger_Name as TAXACCOUNT,a.Tax_Code   from Invoice_Type_TaxHead a left join Accounts.Dbo.Tax_Accounts(" + MyParent.CompCode + ", '" + MyParent.YearCode + "') b on a.Tax_Code =b.Ledger_Code Left join Tax_Settings c on a.Tax_Code = c.Tax_Code  where a.Company_Code = " + MyParent.CompCode + " and a.Rowid = " + TxtInvoiceType.Tag.ToString() + "  and c.per is not null and b.Ledger_Name  Not like '%SGST%' and b.Ledger_Name  Not like '%CGST%'  ", String.Empty, 400);
                            }
                            if (Dr1 != null)
                            {
                                Grid1["TAXACCOUNT", Grid1.CurrentCell.RowIndex].Value = Dr1["TAXACCOUNT"].ToString();
                                Grid1["TAX_CODE", Grid1.CurrentCell.RowIndex].Value = Dr1["Tax_Code"].ToString();
                                DataTable TDt1 = new DataTable();
                                MyBase.Load_Data("Select Dbo.Get_Tax_Per(" + Dr1["Tax_Code"] + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "') ", ref TDt1);
                                Grid1["PERCENTAGE", Grid1.CurrentCell.RowIndex].Value = Convert.ToDouble(TDt1.Rows[0][0]);
                                Txt1.Text = Dr1["TAXACCOUNT"].ToString();
                                TxtAmt.Tag = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "AMOUNT", "ITEM")));
                                TxtGrossAmt.Tag = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "QTY", "ITEM")));
                                if (Convert.ToDouble(TxtAmt.Text) > 0)
                                {
                                    //if (Dr1["Tax_Code"].ToString() != "5890")
                                    //{
                                    Grid1["TAXAMOUNT", Grid1.CurrentCell.RowIndex].Value = Math.Round((Convert.ToDouble(TxtAmt.Text) * Convert.ToDouble(Grid1["PERCENTAGE", Grid1.CurrentCell.RowIndex].Value)) / 100, 0);
                                    //}
                                    //else
                                    //{
                                    //   Grid1["TAXAMOUNT", Grid1.CurrentCell.RowIndex].Value = Math.Round((((Convert.ToDouble(Convert.ToDouble(TxtAmt.Tag) * Convert.ToDouble(TxtGrossAmt.Tag)) * 60 / 100) * Convert.ToDouble(Grid1["PERCENTAGE", Grid1.CurrentCell.RowIndex].Value))/100),0);                                            
                                    //}
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid InvoiceType", "Gainup");
                            return;
                        }
                    }
                    Total_Amount();
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QTY"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["AMOUNT"].Index)
                {
                    if (Grid["ITEM", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        MessageBox.Show("Invalid Item Details ..!", "Gainup");
                        Grid.CurrentCell = Grid["ITEM", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (Grid["OCN_NO", Grid.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        MessageBox.Show("Invalid OCN_NO ..!", "Gainup");
                        Grid.CurrentCell = Grid["OCN_NO", Grid.CurrentCell.RowIndex];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Qty"].Index)
                    {
                        MyBase.Valid_Number(Txt, e);
                    }
                    
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                    {
                        MyBase.Valid_Decimal(Txt, e);
                        return;
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Description"].Index)
                {

                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DISC_PER"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["OCN_NO"].Index)
                {
                    MyBase.Valid_Null(Txt, e);
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["HSN"].Index)
                {
                    MyBase.Valid_Number(Txt, e);
                }
                else
                {
                    MyBase.Valid_Null(Txt, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(Txt1, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["RATE"].Index)
                {
                    if (Grid["QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["QTY", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = "0.00";
                    }
                    else
                    {
                        if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                        {
                            Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = "0.00";
                        }
                        else
                        {
                            Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Txt.Text) * Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value));
                        }
                    }
                }
                else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["QTY"].Index)
                {
                    if (Grid["QTY", Grid.CurrentCell.RowIndex].Value == null || Grid["QTY", Grid.CurrentCell.RowIndex].Value == DBNull.Value || Convert.ToDouble(Grid["QTY", Grid.CurrentCell.RowIndex].Value) == 0)
                    {
                        Grid["QTY", Grid.CurrentCell.RowIndex].Value = "0.00";
                    }
                }
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["DISC_PER"].Index)
                {
                    if (Txt.Text == String.Empty || Convert.ToDouble(Txt.Text) == 0)
                    {
                        Grid["DISC_PER", Grid.CurrentCell.RowIndex].Value = "0.00";
                        Grid["DISC_AMT", Grid.CurrentCell.RowIndex].Value = "0.00";
                    }
                    else
                    {
                        Grid["DISC_AMT", Grid.CurrentCell.RowIndex].Value = (Convert.ToDouble(Grid["AMOUNT", Grid.CurrentCell.RowIndex].Value) * (Convert.ToDouble(Txt.Text) / 100));
                    }
                }
                Total_Amount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Online_Sales_Invoice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name != TxtPortInv.Name && this.ActiveControl.Name != TxtRefNo.Name && this.ActiveControl.Name != TxtShipAddress.Name && this.ActiveControl.Name != TxtBillAddress.Name && this.ActiveControl.Name != TxtAddress.Name && this.ActiveControl.Name != TxtRemarks.Name && this.ActiveControl.Name != TxtAgentCom.Name && this.ActiveControl.Name != String.Empty)
                    {
                        MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                    }
                    else if (this.ActiveControl.Name == "TxtFreightCost")
                    {
                        MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid);
                //if (MyParent._New)
                //{
                //    if (Grid.Rows.Count > 2)
                //    {
                //        if (TxtInvoiceType.Tag.ToString() != "18" && TxtInvoiceType.Tag.ToString() != "19" && TxtInvoiceType.Tag.ToString() != "35")
                //        {
                //            Grid["PONO", Grid.CurrentCell.RowIndex].Value = Grid["PONO", Grid.CurrentCell.RowIndex - 1].Value;
                //            Txt.Text = Grid["PONO", Grid.CurrentCell.RowIndex].Value.ToString();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                if (Grid.Rows.Count > 1)
                {
                    MyBase.Row_Number(ref Grid);
                    //Total_Amount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                //MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                if (Grid.CurrentCell.RowIndex <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        listBox1.Items.Add(Grid["RowID", Grid.CurrentCell.RowIndex].Value.ToString());
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void Grid1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid1, ref Dt1, Grid1.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void Grid1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                MyBase.Row_Number(ref Grid1);
                // Total_Amount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtAmt.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtNetAmt.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total_Amount()
        {
            Double Amount = 0;
            Double Amount1 = 0;
            Double Amount2 = 0;
            Double Amount3 = 0;
            try
            {
                
                //                Amount2 = Convert.ToDouble(MyBase.Sum(ref Grid, "EX_DUTY_AMOUNT", "ITEM"));
                TxtQty.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "Qty", "ITEM")));
                TxtAmt.Text = String.Format("{0:n}", (Convert.ToDouble(MyBase.Sum(ref Grid, "AMOUNT", "ITEM"))) - Convert.ToDouble(MyBase.Sum(ref Grid, "DISC_AMT", "ITEM")));
                TxtGrossAmt.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid1, "TAXAMOUNT", "PERCENTAGE")));
                Amount = (Convert.ToDouble(MyBase.Sum(ref Grid, "AMOUNT", "ITEM"))) - Convert.ToDouble(MyBase.Sum(ref Grid, "DISC_AMT", "ITEM"));
                Amount1 = Convert.ToDouble(MyBase.Sum(ref Grid1, "TAXAMOUNT", "PERCENTAGE"));
                TxtTaxCount.Text = String.Format("{0:n}", Convert.ToInt64(MyBase.Count(ref Grid1, "TAXAMOUNT", "PERCENTAGE")));
                Amount3 = Convert.ToDouble(MyBase.Sum(ref Grid3, "AMOUNT", "CHARGES_NAME"));
                //Amount3 = Convert.ToDouble(TxtFreightCost.Text.ToString()) * Convert.ToDouble(MyBase.Sum(ref Grid, "QTY", "ITEM"));
                TxtChargesAmount.Text = Amount3.ToString();
                TxtNetAmt.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0.00}", Amount + Amount1 + Amount3)));
                //TxtRoAmt.Text = String.Format("{0:n}", Convert.ToDouble(TxtNetAmt.Text) - (Amount + Amount1 + Amount3));
                TxtRoAmt.Text = "0";
                Form_Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Form_Refresh()
        {
            Double Amount = 0;
            Double Amount1 = 0;
            Double Amount3 = 0;
            Double Reb_Amount = 0;
            Double Amount_ActRate = 0.00;
            Double Amount_Tax_ActRate = 0.00;
            try
            {
                if (Convert.ToDouble(TxtAmt.Text) > 0)
                {
                    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    {
                        if (DtpDate.Value >= Convert.ToDateTime("28-jul-2020"))
                        {
                            if (Grid1["TAX_CODE", i].Value.ToString() != "5890")
                            {
                                Grid1["TAXAMOUNT", i].Value = Math.Round((Convert.ToDouble(TxtAmt.Text) * Convert.ToDouble(Grid1["PERCENTAGE", i].Value)) / 100, 2);
                            }
                            else
                            {
                                Grid1["TAXAMOUNT", i].Value = Math.Round((((Convert.ToDouble(Convert.ToDouble(TxtAmt.Tag) * Convert.ToDouble(TxtGrossAmt.Tag)) * 60 / 100) * Convert.ToDouble(Grid1["PERCENTAGE", i].Value)) / 100), 2);
                            }
                        }
                        else
                        {
                            if (Grid1["TAX_CODE", i].Value.ToString() != "5890")
                            {
                                Grid1["TAXAMOUNT", i].Value = Math.Round((Convert.ToDouble(TxtAmt.Text) * Convert.ToDouble(Grid1["PERCENTAGE", i].Value)) / 100, 0);
                            }
                            else
                            {
                                Grid1["TAXAMOUNT", i].Value = Math.Round((((Convert.ToDouble(Convert.ToDouble(TxtAmt.Tag) * Convert.ToDouble(TxtGrossAmt.Tag)) * 60 / 100) * Convert.ToDouble(Grid1["PERCENTAGE", i].Value)) / 100), 0);
                            }
                        }
                    }
                    TxtQty.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "Qty", "ITEM")));
                    TxtAmt.Text = String.Format("{0:n}", (Convert.ToDouble(MyBase.Sum(ref Grid, "AMOUNT", "ITEM")) ) - Convert.ToDouble(MyBase.Sum(ref Grid, "DISC_AMT", "ITEM")));
                    TxtGrossAmt.Text = String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid1, "TAXAMOUNT", "PERCENTAGE")));
                    Amount = (Convert.ToDouble(MyBase.Sum(ref Grid, "AMOUNT", "ITEM")) ) - Convert.ToDouble(MyBase.Sum(ref Grid, "DISC_AMT", "ITEM"));
                    Amount1 = Convert.ToDouble(MyBase.Sum(ref Grid1, "TAXAMOUNT", "PERCENTAGE"));
                    TxtTaxCount.Text = String.Format("{0:n}", Convert.ToInt64(MyBase.Count(ref Grid1, "TAXAMOUNT", "PERCENTAGE")));
                    Amount3 = Convert.ToDouble(MyBase.Sum(ref Grid3, "AMOUNT", "CHARGES_NAME"));
                    //Amount3 = Convert.ToDouble(TxtFreightCost.Text.ToString()) * Convert.ToDouble(MyBase.Sum(ref Grid, "QTY", "ITEM"));
                    TxtChargesAmount.Text = Amount3.ToString();
                    TxtNetAmt.Text = String.Format("{0:n}", Convert.ToDouble(String.Format("{0:0.00}", Amount + Amount1 + Amount3)));
                   // TxtRoAmt.Text = String.Format("{0:n}", Convert.ToDouble(TxtNetAmt.Text) - ((Amount + Amount1 + Amount3)));
                    TxtRoAmt.Text = "0";
                    //if (TxtPartyName.Tag.ToString() == "6555" && Convert.ToDateTime(DtpDate.Value) >= Convert.ToDateTime("26-Nov-2016"))
                    //{
                    //    Reb_Amount = Math.Round((Convert.ToDouble(TxtAmt.Text) * 0.02), 2);
                    //    TxtRoAmt.Text = String.Format("{0:n}", Math.Round(Convert.ToDouble(TxtNetAmt.Text) - Convert.ToDouble(Reb_Amount), 0) - Math.Round(Convert.ToDouble(TxtNetAmt.Text) - Convert.ToDouble(Reb_Amount), 2));
                    //    TxtNetAmt.Text = String.Format("{0:n}", Math.Round(Convert.ToDouble(TxtNetAmt.Text) - Convert.ToDouble(Reb_Amount), 0));
                    //}

                    Amount_ActRate = 0.00;
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid["QTY", i].Value.ToString() != String.Empty && Grid["RATE", i].Value.ToString() != String.Empty)
                        {
                            Amount_ActRate = Convert.ToDouble(String.Format("{0:0.00}", (Amount_ActRate + (Convert.ToDouble(Grid["QTY", i].Value) * Convert.ToDouble(Grid["RATE", i].Value)))));
                        }
                    }
                    Amount_Tax_ActRate = Convert.ToDouble(MyBase.Sum(ref Grid1, "PERCENTAGE", "TAXAMOUNT"));
                    //TxtNetAmtActRate.Text = String.Format("{0:n}", Math.Round(Convert.ToDouble(Amount_ActRate) + Convert.ToDouble(String.Format("{0:0.000}", TxtGrossAmt.Text.ToString())), 0));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtAgentCom_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Decimal(TxtAgentCom, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    SendKeys.Send("{Tab}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtRefNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Return_Ucase(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtRefNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyData == (Keys.Control | Keys.V))
                //    (sender as TextBox).Paste();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Leave(object sender, EventArgs e)
        {
            try
            {
                //return;
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (Convert.ToDouble(Grid["RATE", i].Value) == Convert.ToDouble(Grid["RATE", j].Value) && Grid["Item", i].Value.ToString() == Grid["Item", j].Value.ToString() && Grid["OCN_NO", i].Value.ToString() == Grid["OCN_NO", j].Value.ToString())
                        {
                            MessageBox.Show("Already ITEM, RATE, & OCN_NO is Available", "Gainup");
                            Grid["BOX", i].Value = "0";
                            Grid["QTY", i].Value = "0.000";
                            Grid["RATE", i].Value = "0.00";
                            Grid["OCN_NO", i].Value = "";
                            i = Grid.Rows.Count;
                            j = Grid.Rows.Count;
                            Total_Amount();
                            Grid.CurrentCell = Grid["Ocn_No", j - 2];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtPortInv_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Return_Ucase(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt3 == null)
                {
                    Txt3 = (TextBox)e.Control;
                    Txt3.KeyDown += new KeyEventHandler(Txt3_KeyDown);
                    Txt3.KeyPress += new KeyPressEventHandler(Txt3_KeyPress);
                    Txt3.TextChanged += new EventHandler(Txt3_TextChanged);
                    Txt3.Leave += new EventHandler(Txt3_Leave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    Total_Amount();
                    TxtRemarks.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid3, ref Dt3, Grid3.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void Grid3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (Grid3.Columns.Count > 2)
                {
                    MyBase.Row_Number(ref Grid3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid3_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                if (Grid3.Columns.Count > 2)
                {
                    MyBase.Row_Number(ref Grid3);
                    Total_Amount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == tabControl1.TabPages[1])
                {
                    Grid3.AllowUserToAddRows = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid3.CurrentCell.ColumnIndex == Grid3.Columns["CHARGES_NAME"].Index)
                    {
                        if (TxtInvoiceType.Text.Trim() != string.Empty)
                        {
                            Dr3 = Tool.Selection_Tool_Except_New("CHARGES_NAME", this, 50, 50, ref Dt3, SelectionTool_Class.ViewType.NormalView, "CHARGES", "Select Name CHARGES_NAME,Rowid From Charges_Master  ", String.Empty, 400);
                            if (Dr3 != null)
                            {
                                Grid3["CHARGES_NAME", Grid3.CurrentCell.RowIndex].Value = Dr3["CHARGES_NAME"].ToString();
                                Grid3["Charges_id", Grid3.CurrentCell.RowIndex].Value = Dr3["Rowid"].ToString();
                                Txt3.Text = Dr3["CHARGES_NAME"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid InvoiceType", "Gainup");
                            return;
                        }
                    }

                    Total_Amount();
                }

                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid3.CurrentCell.ColumnIndex == Grid3.Columns["CHARGES_NAME"].Index)
                {
                    MyBase.Valid_Null(Txt3, e);
                }
                else
                {
                    MyBase.Valid_Decimal(Txt3, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Txt3_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Grid3.CurrentCell.ColumnIndex == Grid3.Columns["AMOUNT"].Index)
                {
                    Total_Amount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid3.CurrentCell.ColumnIndex == Grid3.Columns["AMOUNT"].Index)
                {
                    if (Grid3["CHARGES_NAME", Grid3.CurrentCell.RowIndex].Value == DBNull.Value)
                    {
                        Grid3["AMOUNT", Grid3.CurrentCell.RowIndex].Value = 0;
                    }

                }
                Total_Amount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButOk_Click(object sender, EventArgs e)
        {
            DataTable PDt = new DataTable();
            String Str;
            try
            {
                String PEnable = "F";
                if (TxtInvoiceType.Tag.ToString() == "18" || TxtInvoiceType.Tag.ToString() == "19")
                {
                    MyBase.Load_Data("Select A.Ocn_No, A.Jo_qty, A.REc_Qty, A.Inv_Qty, A.Qty Sin_PAck_Qty, A.Bal_Qty from FitSocks.Dbo.Domestic_Invoice_Fgs_Qty_Check_Fn() A Inner Join (Select A.Ocn_No, Sum(A.Single_Per_Pack_Qty) Inv_Qty, Sum(A.Single_Per_Pack_Qty) Qty from Accounts.Dbo.Mis_Domestic_Invoice_Qty() A LEft Join FitSocks.Dbo.Enable_INvoice_Print_Ocn_List B On A.Ocn_No = B.Ocn_No Where A.Rowid = " + Code + " and B.Ocn_NO Is Null Group by A.Ocn_No) B On A.Ocn_No = B.Ocn_No Where Bal_Qty <0 ", ref PDt);
                    if (PDt.Rows.Count > 0)
                    {
                        DataTable TDtp1 = new DataTable();
                        MyBase.Load_Data("Select Invoice_RoWId, RndNo From Invoice_Print_Status_OutPass Where Invoice_rowid = " + Code + " and Type = 'ONLINE'", ref TDtp1);
                        if (TDtp1.Rows.Count == 0)
                        {
                            PEnable = "F";
                        }
                        else
                        {
                            PEnable = "T";
                        }
                    }
                    else
                    {
                        PEnable = "T";
                    }
                }
                else
                {
                    PEnable = "T";
                }


                DataTable TDtp = new DataTable();
                if (MyBase.Get_RecordCount("Invoice_Print_Status", "Invoice_No =  '" + TxtInvoiceNo.Text + "' and  RowID = " + TxtInvoiceNo.Tag + "  and Company_Code = " + MyParent.CompCode + " and Year_Code = '" + MyParent.YearCode + "'") == 0)
                {
                    MyBase.Run("Insert Into Invoice_Print_Status Values('" + TxtInvoiceNo.Text + "'," + TxtInvoiceNo.Tag + ",1," + MyParent.CompCode + ",'" + MyParent.YearCode + "')");
                }

                String OthChg = "";
                if (RBGstPrint.Checked == true || RBGstPrint1.Checked == true)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                  
                        Str = " Select top 10000000 RNo, InvoiceNo, InvoiceDt, GrossAmount, Party_Invoice_No     ,  Party_Invoice_Date,  SupplyPlace, Roundedoff, NetAmount, State, StateCode, '" + TxtAddress.Text.ToString() + "' DELIAT, PGstNo, CState, CStateCode, CGstNo, RevCharge, SNO, CNTNAME  CNTNAME, PACKS, QTY, RATE, AMT, OTHERNAME, Discount, PName, PAddress, Description, UOM, Entry_Date, Lot, Tax_Value, Cgst_Rate, Cgst_Amount, Sgst_Rate, Sgst_Amount, Igst_Rate, Igst_Amount,  HsnCode From S_Online_Sales_Invoice_Gst_Rpt(" + MyParent.CompCode + ",'" + MyParent.YearCode + "', " + TxtInvoiceNo.Tag + ") ORder by SNo ";
                   

                    MyBase.Execute_Qry(Str, "GSTQRYYARNINVOICE_Online");
                    //if (RBGstPrint1.Checked == true)
                    //{
                    ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Rpt_Sales_Online_Invoice.rpt");

                    MyParent.FormulaFill(ref ORpt, "WordsRupee", "" + MyBase.Rupee(Convert.ToDouble(TxtNetAmt.Text)) + "");
                    MyParent.FormulaFill(ref ORpt, "TTamt", (TxtNetAmt.Text.ToString()));

                    MyParent.CReport(ref ORpt, "SALES INVOICE PREPRINT..!");
                    return;
                    //}
                    //else
                    //{
                        //if (TxtPartyName.ToString().ToUpper().Contains("MARKS") != true)
                        //{
                        //    ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Gst2_Socks.rpt");
                        //}
                        //else
                        //{
                        //    ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Gst2_Socks_MNS.rpt");
                        //    //ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Gst2_Socks.rpt"); 
                        //}
                    //}
                    //if (MyParent.CompCode == 1)
                    //{
                    //    MyParent.FormulaFill(ref ORpt, "GSTNo", "33AACCG8906G1ZQ");
                    //}
                    //else if (MyParent.CompCode == 2)
                    //{
                    //    MyParent.FormulaFill(ref ORpt, "GSTNo", "33AACCG8906G1ZQ");
                    //}
                    //else
                    //{
                    //    MyParent.FormulaFill(ref ORpt, "GSTNo", "33AACCG8906G1ZQ");
                    //}
                    //MyParent.FormulaFill(ref ORpt, "Reverse", "No");

                    //for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    //{
                    //    if ((Grid1["TaxAccount", i].Value.ToString().Contains("CGST")))
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Cgst_Per", Grid1["PERCENTAGE", i].Value.ToString());
                    //        MyParent.FormulaFill(ref ORpt, "Cgst_Amt", Grid1["TAXAMOUNT", i].Value.ToString());
                    //    }
                    //    else if ((Grid1["TaxAccount", i].Value.ToString().Contains("SGST")))
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Sgst_Per", Grid1["PERCENTAGE", i].Value.ToString());
                    //        MyParent.FormulaFill(ref ORpt, "Sgst_Amt", Grid1["TAXAMOUNT", i].Value.ToString());
                    //    }
                    //    else if ((Grid1["TaxAccount", i].Value.ToString().Contains("IGST")))
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Igst_Per", Grid1["PERCENTAGE", i].Value.ToString());
                    //        MyParent.FormulaFill(ref ORpt, "Igst_Amt", Grid1["TAXAMOUNT", i].Value.ToString());
                    //    }
                    //}


                    //if (MyParent.CompCode == 1)
                    //{
                    //    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    //    {
                    //        if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 3529)
                    //        {
                    //            MyParent.FormulaFill(ref ORpt, "Others1", "Mark Comm Cess   " + Grid1["PERCENTAGE", i].Value + " %                          " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                    //        }
                    //        else if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 747)
                    //        {
                    //            MyParent.FormulaFill(ref ORpt, "Others2", "TCS              " + Grid1["PERCENTAGE", i].Value + " %                           " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                    //        }
                    //    }
                    //}
                    //else if (MyParent.CompCode == 3)
                    //{
                    //    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    //    {
                    //        if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 3529)
                    //        {
                    //            MyParent.FormulaFill(ref ORpt, "Others1", "Mark Comm Cess   " + Grid1["PERCENTAGE", i].Value + " %                          " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                    //        }
                    //        else if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 747)
                    //        {
                    //            MyParent.FormulaFill(ref ORpt, "Others2", "TCS              " + Grid1["PERCENTAGE", i].Value + " %                           " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                    //    {
                    //        if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 3529)
                    //        {
                    //            MyParent.FormulaFill(ref ORpt, "Others1", "Mark Comm Cess   " + Grid1["PERCENTAGE", i].Value + " %                          " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                    //        }
                    //        else if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 747)
                    //        {
                    //            MyParent.FormulaFill(ref ORpt, "Others2", "TCS              " + Grid1["PERCENTAGE", i].Value + " %                           " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                    //        }
                    //    }
                    //}

                    //if (TxtPartyName.Tag.ToString() == "9107" && MyParent.CompCode == 1)
                    //{
                    //    MyParent.FormulaFill(ref ORpt, "DeptCode", "Department Code - T10");
                    //    MyParent.FormulaFill(ref ORpt, "Carton", "No of Cartons   - " + String.Format("{0:n}", Convert.ToDouble(MyBase.Sum(ref Grid, "BOX", "ORDERNO")).ToString()) + "");
                    //}
                    //for (int i = 0; i <= Dt3.Rows.Count - 1; i++)
                    //{
                    //    if (Convert.ToDouble(Grid3["Charges_ID", i].Value) == 6 || Convert.ToDouble(Grid3["Charges_ID", i].Value) == 3)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Others3", "" + Grid3["Charges_Name", i].Value + "                     " + Grid3["Amount", i].Value + " ");
                    //    }
                    //    else if (Convert.ToDouble(Grid3["Charges_ID", i].Value) == 5)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Others4", "" + Grid3["Charges_Name", i].Value + "                   " + ((TxtChargesAmount.Text).ToString()) + " ");
                    //    }
                    //}
                    
                    //MyParent.FormulaFill(ref ORpt, "NetAmt", String.Format("{0:n}", Convert.ToDouble(TxtNetAmt.Text).ToString()));
                    //MyParent.FormulaFill(ref ORpt, "GrossAmt", String.Format("{0:n}", (String.Format("{0:0}", Convert.ToDouble(TxtAmt.Text).ToString()))));

                
                }


                bool ischecked = RBWORD.Checked;
                bool ischecked1 = RBOTHER.Checked;
                if (ischecked || ischecked1)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    //Exec Socks_Order_Close_After_Invoice_Approval_Style_Domestic 
                    MyBase.Run("Exec Accounts.Dbo.Socks_Order_Close_After_Online_Invoice_Approval_Style_Domestic " + Code + " ");
                    if (ischecked)
                    {
                        Str = " Select Invoice_No INVOICENO, Invoice_date INVOICEDT, Invoice_Type, '' PERMITNO, '' QTAXAMT, '' DESPTHRU, '' DESPINSTR, '' LRRNO, In_Gross_Amt GROSSAMOUNT, '' PREMITPER, '' PREMITAMT, '' SCPER, '' SCAMT, 0 STAXAMT, '' OTHER1PER, '' OTHER1AMT, In_TNet_Amt NETAMOUNT, Delivery_Address DELIVAT, '' TAX3PER, '' TAX3AMT, '' TAX4PER, '' TAX4AMT, Entry_Date INVOICETIME, '' DELIVERYTIME, '' FREPER, '' FREAMT, Party LEDGERNAME, Item WASNAME, DESCRIPTION, (Item + ' - ' + DESCRIPTION )ITEM, In_Qty PACKS, In_Qty QTY, RATE, Amount AMT,UOM, '' OTH1LEDGERNAME, '' OTH2LEDGERNAME, Party_Address LADDRESS, TinNo LLSTNO, CSTNo LCSTNO, TinNo LECCNO, In_RO_Amt ROUND, '' QAMOUNT, '' QINVNO, '' QRATE, '' FREIGHTNAME, '' EDNAME, '' HCESSNAME, '' ECESSNAME, Rate MRP, 0 EX_DUTY, 0 Ex_Duty_Amount From S_Online_Sales_Invoice_Fn_WoTax_Print(" + MyParent.CompCode + ",'" + MyParent.YearCode + "') S Where In_MAster_ID = " + TxtInvoiceNo.Tag + "";
                        MyBase.Execute_Qry(Str, "QRYWASTEBILL");
                        if (Convert.ToDouble(Grid["RATE", 0].Value) > 0)
                        {
                            ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Socksbillpre_Socks_New_MRP.rpt");
                        }
                        else
                        {
                            ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Socksbillpre_Socks_New.rpt");
                        }
                    }
                    else
                    {
                        Str = " Select Invoice_No INVOICENO, Invoice_date INVOICEDT, Invoice_Type, '' PERMITNO, '' QTAXAMT, '' DESPTHRU, '' DESPINSTR, '' LRRNO, In_Gross_Amt GROSSAMOUNT, '' PREMITPER, '' PREMITAMT, '' SCPER, '' SCAMT, cast(In_Other_Amt as Numeric(25,2)) STAXAMT, '' OTHER1PER, '' OTHER1AMT, In_TNet_Amt NETAMOUNT, Delivery_Address DELIVAT, '' TAX3PER, '' TAX3AMT, '' TAX4PER, '' TAX4AMT, Entry_Date INVOICETIME, '' DELIVERYTIME, '' FREPER, '' FREAMT, Party LEDGERNAME, Item WASNAME, DESCRIPTION, (Item + ' - ' + DESCRIPTION )ITEM, In_Qty PACKS, In_Qty QTY, RATE, Amount AMT, UOM, '' OTH1LEDGERNAME, '' OTH2LEDGERNAME, Party_Address LADDRESS, TinNo LLSTNO, CSTNo LCSTNO, TinNo LECCNO, In_RO_Amt ROUND, '' QAMOUNT, '' QINVNO, '' QRATE, '' FREIGHTNAME, '' EDNAME, '' HCESSNAME, '' ECESSNAME, 0 MRP, 0 EX_DUTY, 0 Ex_Duty_Amount, Rate Act_Rate, In_Qty * Rate Act_Amount From S_Online_Sales_Invoice_Fn_WoTax_Print(" + MyParent.CompCode + ",'" + MyParent.YearCode + "') S Where In_MAster_ID = " + TxtInvoiceNo.Tag + "";
                        MyBase.Execute_Qry(Str, "QRYWASTEBILL_TAXBILL");
                        ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Socksbillpre_Socks_New_Tax.rpt");
                    }
                    MyParent.FormulaFill(ref ORpt, "invno", "" + TxtInvoiceNo.Text + "");
                    //for (int i = 0; i <= Dt2.Rows.Count - 1; i++)
                    //{
                    //    if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 7)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "DESPTHRU", "" + Grid2["Description", i].Value + "");
                    //    }
                    //    else if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 8)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "LORRYNO", "" + Grid2["Description", i].Value + "");
                    //    }
                    //    else if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 32)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "LRRNO", "" + Grid2["Description", i].Value + "");
                    //    }
                    //    else if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 33)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "DESPINSTR", "" + Grid2["Description", i].Value + "");
                    //    }
                    //}


                    if (MyParent.CompCode == 1)
                    {
                        for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 5498 || Convert.ToDouble(Grid1["Tax_Code", i].Value) == 746)
                            {
                                MyParent.FormulaFill(ref ORpt, "Vat", "OUTPUT VAT   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                            else if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 5526 || Convert.ToDouble(Grid1["Tax_Code", i].Value) == 5229)
                            {
                                MyParent.FormulaFill(ref ORpt, "Cst", "CST  @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                            else if (Grid1["TAXACCOUNT", i].Value.ToString().Contains("TCS") == true)
                            {
                                MyParent.FormulaFill(ref ORpt, "Tcs", "TCS   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                            else if ((Grid1["TaxAccount", i].Value.ToString().Contains("CGST")))
                            {
                                MyParent.FormulaFill(ref ORpt, "Vat", "Cgst_Per   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                            else if ((Grid1["TaxAccount", i].Value.ToString().Contains("SGST")))
                            {
                                MyParent.FormulaFill(ref ORpt, "Cst", "Sgst_Per   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                            else if ((Grid1["TaxAccount", i].Value.ToString().Contains("IGST")))
                            {
                                MyParent.FormulaFill(ref ORpt, "Tcs", "Igst_Per   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                            else
                            {
                                MyParent.FormulaFill(ref ORpt, "TaxOth", "" + Grid1["TAXACCOUNT", i].Value + "   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                            }
                        }
                    }

                    if (ischecked1)
                    {
                        if (MyParent.CompCode == 1)
                        {
                            for (int i = 0; i <= Dt1.Rows.Count - 1; i++)
                            {
                                if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 5498 || Convert.ToDouble(Grid1["Tax_Code", i].Value) == 746)
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "Vat", "OUTPUT VAT   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                                else if (Convert.ToDouble(Grid1["Tax_Code", i].Value) == 5526 || Convert.ToDouble(Grid1["Tax_Code", i].Value) == 5229)
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "Cst", "CST  @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                                else if (Grid1["TAXACCOUNT", i].Value.ToString().Contains("TCS") == true)
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "Tcs", "TCS   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                                else if ((Grid1["TaxAccount", i].Value.ToString().Contains("CGST")))
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "Vat", "Cgst_Per   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                                else if ((Grid1["TaxAccount", i].Value.ToString().Contains("SGST")))
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "Cst", "Sgst_Per   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                                else if ((Grid1["TaxAccount", i].Value.ToString().Contains("IGST")))
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "Tcs", "Igst_Per   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                                else
                                {
                                    MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "TaxOth", "" + Grid1["TAXACCOUNT", i].Value + "   @" + Grid1["PERCENTAGE", i].Value + " %    " + String.Format("{0:0.00}", Convert.ToDouble(Grid1["TAXAMOUNT", i].Value)) + "");
                                }
                            }
                        }

                        MyParent.FormulaFill_Sub(ref ORpt, "rptsocksbillvalue", "PackCharge", ((TxtChargesAmount.Text).ToString()));
                    }
                    Total_Amount();
                    Reb_Amount = Math.Round(((Convert.ToDouble(TxtAmt.Text) - Convert.ToDouble(0)) * 0.02), 2);
                    //if(TxtPartyName.Tag.ToString() == "6555" && Convert.ToDateTime(DtpDate.Value) >= Convert.ToDateTime("26-Nov-2016")  && Convert.ToInt64(TxtSoNo.Tag) > 1068) 
                    //{        
                    //    Double MRP_Amount = 0;                        
                    //    MRP_Amount = Convert.ToDouble(MyBase.Sum(ref Grid, "EX_DUTY_AMOUNT", "ORDERNO"));
                    //    Reb_Amount = Math.Round( ((Convert.ToDouble(TxtAmt.Text) - Convert.ToDouble(MRP_Amount)) *  0.02),2);
                    //}
                    //else
                    //{
                    //        Reb_Amount = 0;
                    //}
                    if (TxtPartyName.Tag.ToString() == "6555")
                    {
                        MyParent.FormulaFill(ref ORpt, "RebAmt", "REBATE @2 %   " + ((Reb_Amount.ToString())));
                    }
                    else
                    {
                        MyParent.FormulaFill(ref ORpt, "RebAmt", "  ");
                    }


                    for (int i = 0; i <= Dt3.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDouble(Grid3["Charges_ID", i].Value) == 3)
                        {
                            MyParent.FormulaFill(ref ORpt, "RebAmt", "" + Grid3["Charges_Name", i].Value + "     " + Grid3["Amount", i].Value + " ");
                        }
                        //else if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 34)
                        //{
                        //    MyParent.FormulaFill(ref ORpt, "Division", "" + Grid2["Description", i].Value + "");
                        //    i = Dt2.Rows.Count;
                        //}
                    }

                    MyParent.FormulaFill(ref ORpt, "WordsRupee", MyBase.Rupee(Convert.ToDouble(TxtNetAmt.Text)));
                    MyParent.CReport(ref ORpt, "SALES INVOICE PREPRINT..!");
                    return;
                }

                bool ischecked3 = RBOUTPASS.Checked;
                bool ischecked4 = RBDC.Checked;
                if (ischecked3 || ischecked4)
                {
                    String FOcn = String.Empty;
                    if (PEnable == "T")
                    {
                        if (MyBase.Get_RecordCount("Invoice_Print_Status_OutPass", "Invoice_RowID = " + Code + "") == 0)
                        {
                            MyBase.Run("Insert Into Invoice_Print_Status_OutPass (Invoice_RoWId, Type) Values(" + Code + ", 'SOCKS')");
                        }
                    }
                    else
                    {
                        if (PDt.Rows.Count == 1)
                        {
                            MessageBox.Show("Invalid FGS Qty in [" + PDt.Rows[0]["Ocn_No"] + "]", "Gainup");
                        }
                        else if (PDt.Rows.Count > 1)
                        {
                            FOcn = "";
                            for (int k = 0; k < PDt.Rows.Count; k++)
                            {
                                FOcn = FOcn + "[" + PDt.Rows[k]["Ocn_No"] + "];";
                            }
                            MessageBox.Show("Invalid FGS Qty in " + FOcn + "", "Gainup");
                        }
                        if (MyParent.UserCode != 1)
                        {
                            //if (Convert.ToDateTime("29-jan-2018").Date  <= MyBase.GetServerDate())
                            //{                            
                            return;
                            //}    
                        }
                    }
                    Str = " Select Distinct Invoice_No InvoiceNo, Invoice_Date InvoiceDt, 0 TaxPer, In_Gross_Amt GrossAmount, 0 Tax,  0 OTHER1, In_Ro_Amt Roundedoff, In_TNet_Amt NetAmount, 0 as cessamt, 0 cessper, Delivery_Address DELIAT, 0 as chipstatus, 0 as chipval,  In_Sno SNO,  (Item + ' - ' + DESCRIPTION ) CNTNAME,cast(in_Qty as int)QTY ,In_QTY PACKS, RATE,Amount AMT, '' CESSNAME, UOM OTHERNAME, Tinno TINNO, CSTNo CSTNO, Party PARTYNAME, Party_Address PAddress, Description, UOM   From S_Online_Sales_Invoice_Fn_WoTax_Print(" + MyParent.CompCode + ",'" + MyParent.YearCode + "') S Where In_MAster_ID = " + TxtInvoiceNo.Tag + "";
                    MyBase.Execute_Qry(Str, "QRYYARNINVOICE1");

                    CrystalDecisions.CrystalReports.Engine.ReportDocument ORpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    if (ischecked3)
                    {
                        ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Gpass_New_S.rpt");
                    }
                    else
                    {
                        ORpt.Load(System.Windows.Forms.Application.StartupPath + "\\Gpass_New_DC.rpt");
                    }
                    MyParent.FormulaFill(ref ORpt, "Head1", MyParent.CompName);
                    MyParent.FormulaFill(ref ORpt, "Head2", MyParent.CompAddress);
                    MyParent.FormulaFill(ref ORpt, "invno", "" + TxtInvoiceNo.Text + "");
                    //for (int i = 0; i <= Dt2.Rows.Count - 1; i++)
                    //{
                    //    if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 8)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Lorryno", "" + Grid2["Description", i].Value + "");
                    //    }
                    //    else if (Convert.ToDouble(Grid2["Terms_Id", i].Value) == 34)
                    //    {
                    //        MyParent.FormulaFill(ref ORpt, "Division", "" + Grid2["Description", i].Value + "");
                    //        i = Dt2.Rows.Count;
                    //    }
                    //}

                    MyParent.FormulaFill(ref ORpt, "CST", "CST NO : " + MyParent.CompCst + "");
                    MyParent.FormulaFill(ref ORpt, "TIN", "TIN NO : " + MyParent.Company_Tin + "");
                    MyParent.FormulaFill(ref ORpt, "PHONE", "PHONE   : " + MyParent.CompPhone + "");
                    MyParent.FormulaFill(ref ORpt, "CFAX", "FAX   :" + MyParent.CompFax + "");
                    DataTable TDtp1 = new DataTable();
                    MyBase.Load_Data("Select Invoice_RoWId, RndNo From  Invoice_Print_Status_OutPass Where Invoice_rowid = " + Code + " and Type = 'SOCKS'", ref TDtp1);
                    if (TDtp1.Rows.Count > 0)
                    {
                        MyParent.FormulaFill(ref ORpt, "RndNo", TDtp1.Rows[0][1].ToString());
                    }
                    else
                    {
                        MyParent.FormulaFill(ref ORpt, "RndNo", "");
                    }
                    MyParent.CReport(ref ORpt, "SALES INVOICE GATEPASS..!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                GBReport.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}