using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace cfg_ss
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string temp1;
        public string temp2;
        public string temp;
        public int line_No = 1;
        
        Dictionary<string, string> Keywordss = new Dictionary<string, string>()
        {
          {"Main" , "Main"} , {"void", "void"} ,{"MainClass" , "MainClass"} ,  {"true" , "Bool_Const"} ,  {"false" , "Bool_Const"}, {"int" , "DT"} , {"float", "DT"} , {"char", "DT"} , {"string","DT"} , {"bool","DT"} , {"Floop","Floop"} , {"Wloop","Wloop"} , {"Jump","Jump"} , {"Break","Break"}, {"Switch","Switch"} , {"Case","Case"}, {"Default","Default"} , {"Class","Class"} , {"public","AM"} , {"private","AM"} ,{"If","If"},{"Iff","Iff" },{"Else","Else"},{"return","return"},{"Sealed","Sealed"},{"Abstract","Abstract"},{"EndFloop","EndFloop"},{"EndWloop","EndWloop"} ,{"override","VO"},{"virtual","VO"},{"new","new"}
        };
        Dictionary<string, string> Opr = new Dictionary<string, string>()
        {
            {"+","PM" } , {"-","PM"} , {"*","MDM"} , {"/","MDM"} , {"%","MDM"} , {"=","="} , {"+=","Asgn_Op"} , {"-=","Asgn_Op"} , {"*=","Asgn_Op"} , {"/=","Asgn_Op"} , {"%=","Asgn_Op"} , {"++","INC_DEC"} , {"--","INC_DEC"} , {"&&","&&"} , {"||","||"} , {"<","ROP"} , {">","ROP"} , {"<<","SO"} , {">>","SO"} , {"==","ROP"} , {"!=","ROP"}, {"!","UNO"}, {"<=","ROP"} , {">=","ROP"}
        };
        Dictionary<string, string> Punct = new Dictionary<string, string>
        {
            { "." , "."} , {",",","} , {";",";"} , {"(","("} ,{")",")"} , {"{","{"} , {"}","}"} , {"[","["} , {"]","]"} , {"::","::"} , {":",":"}
        };

        //Regex ID = new Regex("^[_a-zA-Z][a-zA-Z0-9]{0,}|([a-zA-Z0-9][_][a-zA-Z0-9]*)+$");
        Regex ID = new Regex("^[_a-zA-Z]{1}[_a-zA-Z0-9]{0,}$");
        Regex Int_Const = new Regex("^[-+][0-9]+$|^[0-9]+$|^[0-9][-+][0-9]$");
        Regex Float_Const = new Regex("^[+-][0-9]+$|^[0-9]+$|^[+-][0-9]+[.][0-9]+$|^[0-9]*[.][0-9]+$");
        Regex Char_Const = new Regex(@"^'[a-zA-Z0-9]'$|^'[+-]'$|^'\\[nrt]'$");
        Regex Str_Const = new Regex("^\"([a-zA-Z0-9]|(\\\\\\\\)|[!@#$%^&*()-=+{}|;:<>,.?/']|\\[|\\]|_|\\\\[nrtfab0]|\\\\\"|\\\\')*\"$");

        bool A1 = false;
        bool A2 = false;
        bool A3 = false;
        bool A4 = false;
        bool A5 = false;
        bool A6 = false;
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox3.Text = null;
            ArrayList mini_token = new ArrayList();

            ArrayList tokenss = new ArrayList();
            ArrayList naya_token = new ArrayList();
            string Inp_Str = richTextBox1.Text;

            for (int y = 0; y < Inp_Str.Length; y++)
            {
            Start_Pos:
                if (y == Inp_Str.Length)
                {
                    break;
                }


                if (Inp_Str[y] == '.' && Inp_Str[y + 1] == '1' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '2' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '3' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '4' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '5' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '6' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '7' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '8' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '9' ||
                        Inp_Str[y] == '.' && Inp_Str[y + 1] == '0')
                {
                    goto float_check;
                }


                if (Inp_Str[y] != '"' && Inp_Str[y] != ' ' && Inp_Str[y] != ',' && Inp_Str[y] != '\'' && Inp_Str[y] != ';'
                && Inp_Str[y] != '=' && Inp_Str[y] != '!' && Inp_Str[y] != '\n' && Inp_Str[y] != '.' && Inp_Str[y] != ':'
                && Inp_Str[y] != '+' && Inp_Str[y] != '-' && Inp_Str[y] != '*' && Inp_Str[y] != '/' && Inp_Str[y] != '%'
                && Inp_Str[y] != '(' && Inp_Str[y] != ')' && Inp_Str[y] != '{' && Inp_Str[y] != '}' && Inp_Str[y] != '['
                && Inp_Str[y] != ']' && Inp_Str[y] != '<' && Inp_Str[y] != '>')
                {
                    temp = temp + Inp_Str[y];
                    if (y == Inp_Str.Length - 1)
                    {
                        temp = temp.Trim();
                        mini_token.Add(temp);
                    }
                    y++;
                    goto Start_Pos;
                }

                mini_token.Add(temp);
                temp = "";



                //complex check
                if (Inp_Str[y] == '+' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '-' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '*' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '/' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '%' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '=' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '>' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '<' && Inp_Str[y + 1] == '=' ||
                    Inp_Str[y] == '>' && Inp_Str[y + 1] == '>' ||
                    Inp_Str[y] == '<' && Inp_Str[y + 1] == '<' ||
                    Inp_Str[y] == '+' && Inp_Str[y + 1] == '+' ||
                    Inp_Str[y] == '-' && Inp_Str[y + 1] == '-' ||
                    Inp_Str[y] == ':' && Inp_Str[y + 1] == ':' ||
                    Inp_Str[y] == '!' && Inp_Str[y + 1] == '=')
                {
                    temp = Convert.ToString(Inp_Str[y]) + Convert.ToString(Inp_Str[y + 1]);
                    temp = temp.Trim();
                    mini_token.Add(temp);
                    temp = "";
                    y += 2;
                    goto Start_Pos;
                }

                //char breaking

                if (Inp_Str[y] == '\'')
                {
                Chr_Start_Pos:
                    if (y == Inp_Str.Length)
                    {
                        temp = temp.Trim();
                        mini_token.Add(temp);
                        break;
                    }

                    temp = temp + Inp_Str[y];
                    y++;
                    if (y == Inp_Str.Length)
                    {
                        temp = temp.Trim();
                        mini_token.Add(temp);
                        break;
                    }

                    if (Inp_Str[y] == '\\')
                    {
                        if (y == Inp_Str.Length - 1)
                        {
                            temp = temp + Inp_Str[y];
                            temp = temp.Trim();
                            mini_token.Add(temp);
                            temp = "";
                            break;
                        }
                        if (y == Inp_Str.Length - 2)
                        {
                            temp = temp + Inp_Str[y] + Inp_Str[y + 1];
                            temp = temp.Trim();
                            mini_token.Add(temp);
                            temp = "";
                            break;
                        }

                        if (Inp_Str[y + 2] == '\'')
                        {
                            temp = temp + Inp_Str[y] + Inp_Str[y + 1] + Inp_Str[y + 2];
                            y += 3;
                            goto Start_Pos;
                        }
                        if (Inp_Str[y + 2] != '\'')
                        {
                            temp = temp + Inp_Str[y] + Inp_Str[y + 1] + Inp_Str[y + 2];
                            y += 3;
                            goto Start_Pos;
                        }

                    }
                    if (Inp_Str[y] != '\\')
                    {
                        if (y == Inp_Str.Length - 1)
                        {
                            temp = temp + Inp_Str[y];
                            temp = temp.Trim();
                            mini_token.Add(temp);
                            temp = "";
                            break;
                        }

                        if (Inp_Str[y + 1] == '\'')
                        {
                            temp = temp + Inp_Str[y] + Inp_Str[y + 1];
                            y += 2;
                            goto Start_Pos;
                        }
                        if (Inp_Str[y + 1] != '\'')
                        {
                            temp = temp + Inp_Str[y] + Inp_Str[y + 1];
                            y += 2;
                            temp = temp.Trim();
                            mini_token.Add(temp);
                            temp = "";
                            goto Start_Pos;
                        }

                    }
                    if (Inp_Str[y] == '\n')
                    {
                        temp = temp.Trim();
                        mini_token.Add(temp);
                        temp = "";
                        goto Chr_Start_Pos;
                    }
                    goto Chr_Start_Pos;
                }

                //String breaking

                if (Inp_Str[y] == '"')
                {
                str_start:
                    temp = temp + Inp_Str[y];

                    if (y == (Inp_Str.Length - 1))
                    {
                        mini_token.Add(temp);
                        temp = "";
                        break;
                    }

                    y++;

                    if (Inp_Str[y] == '\n')
                    {
                        mini_token.Add(temp);
                        temp = "";
                        goto Start_Pos;
                    }

                    if (Inp_Str[y] == '\\')
                    {
                        temp = temp + Inp_Str[y];
                        y++;
                        goto str_start;
                    }

                    if (Inp_Str[y] == '"')
                    {
                        temp = temp + Inp_Str[y];
                        mini_token.Add(temp);
                        temp = "";
                        y++;
                        goto Start_Pos;
                    }
                    else
                    {
                        goto str_start;
                    }
                }




                //Single Line Comment
                if (Inp_Str[y] == '/' && Inp_Str[y + 1] == '/')
                {
                    y += 2;

                here:
                    if (y == Inp_Str.Length)
                    {
                        break;
                    }

                    if (Inp_Str[y] == '\n')
                    {

                        goto Start_Pos;
                    }

                    y++;
                    goto here;
                }

                //multi-line Comment
                if (Inp_Str[y] == '/' && Inp_Str[y + 1] == '*')
                {
                    temp = null;
                    y += 2;
                db_Comment:
                    if (Inp_Str[y] == '*' && Inp_Str[y + 1] == '/')
                    {
                        y += 2;
                        if (y == Inp_Str.Length)
                        {
                            break;
                        }
                        goto Start_Pos;
                    }
                    if (Inp_Str[y] == '\n')
                    {
                        line_No++;
                    }
                    y++;
                    goto db_Comment;
                }

                //word break equal 
                if (Inp_Str[y] == ' ' || Inp_Str[y] == ',' || Inp_Str[y] == ';' || Inp_Str[y] == '=' || Inp_Str[y] == '!' || Inp_Str[y] == '\n' || Inp_Str[y] == '.' || Inp_Str[y] == ':' || Inp_Str[y] == '+' || Inp_Str[y] == '-' || Inp_Str[y] == '*' || Inp_Str[y] == '/' || Inp_Str[y] == '%' || Inp_Str[y] == '(' || Inp_Str[y] == ')' || Inp_Str[y] == '{' || Inp_Str[y] == '}' || Inp_Str[y] == '[' || Inp_Str[y] == ']' || Inp_Str[y] == '<' || Inp_Str[y] == '>')
                {
                    if (Inp_Str[y] != ' ' && Inp_Str[y] != '\n')
                    {
                        temp = "";
                        temp = Convert.ToString(Inp_Str[y]);
                        temp = temp.Trim();
                        mini_token.Add(temp);
                        temp = "";
                        y++;
                        goto Start_Pos;
                    }
                }











            float_check:
                if (Inp_Str[y] == '.' && Inp_Str[y + 1] == '1' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '2' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '3' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '4' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '5' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '6' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '7' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '8' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '9' ||
                    Inp_Str[y] == '.' && Inp_Str[y + 1] == '0')
                {

                    if (temp == "")
                    {
                        goto dot_creat;
                    }

                    temp = "";

                temp1_creat:
                    y--;

                    if (y == -1)
                    {
                        goto dot_enter;

                    }

                    else if (Inp_Str[y] != '=' && Inp_Str[y] != ' ' && Inp_Str[y] != '\n' && Inp_Str[y] != '.')

                    {
                        temp1 = Inp_Str[y] + temp1;
                        goto temp1_creat;

                    }




                dot_enter:
                    y++;
                dot_creat:

                    if (Inp_Str[y] != '.')
                    {
                        y++;
                        goto dot_creat;
                    }


                    temp = Convert.ToString(Inp_Str[y]);


                temp2_creat:

                    y++;
                    if (y == Inp_Str.Length)
                    {
                        goto regex_passing;

                    }

                    if (Inp_Str[y] != '=' && Inp_Str[y] != ' ' && Inp_Str[y] != '\n' && Inp_Str[y] != '.')
                    {
                        temp2 = temp2 + Inp_Str[y];
                        goto temp2_creat;
                    }

                regex_passing:
                    bool result_temp1 = Int_Const.IsMatch(Convert.ToString(temp1));
                    bool result_temp2 = Int_Const.IsMatch(Convert.ToString(temp2));

                    if (result_temp1 == true && result_temp2 == true)
                    {
                        //  pre_tokens.Add(temp1 + temp + temp2);
                        mini_token.Add(temp1 + temp + temp2);
                        temp = "";
                        temp1 = "";
                        temp2 = "";
                    }

                    if (result_temp1 == false && result_temp2 == true)
                    {

                        // pre_tokens.Add(temp1);
                        //pre_tokens.Add(temp + temp2);
                        mini_token.Add(temp1);
                        mini_token.Add(temp + temp2);
                        temp = "";
                        temp1 = "";
                        temp2 = "";
                    }

                    if (result_temp1 == false && result_temp2 == false)
                    {
                        //   pre_tokens.Add(temp1);
                        // pre_tokens.Add(temp);
                        //pre_tokens.Add(temp2);
                        mini_token.Add(temp1);
                        mini_token.Add(temp + temp2);
                        temp = "";
                        temp1 = "";
                        temp2 = "";
                    }

                    if (result_temp1 == true && result_temp2 == false)
                    {
                        //pre_tokens.Add(temp1);
                        //pre_tokens.Add(temp);
                        //pre_tokens.Add(temp2);
                        mini_token.Add(temp1);
                        mini_token.Add(temp + temp2);
                        temp = "";
                        temp1 = "";
                        temp2 = "";
                    }

                    goto Start_Pos;
                }









                //Enter logic
                if (Inp_Str[y] == '\n')
                {
                    mini_token.Add("Enter");

                    temp = null;
                }
            }//for loop ends
            //foreach (string s in mini_token)
            //{
            //    richTextBox2.Text = richTextBox2.Text + '\n' + s;
            //}
            //alag textbox me space r enter k bghr
            for (int l = 0; l < mini_token.Count; l++)
            {
                if (mini_token[l] != " " && mini_token[l] != "\n" && mini_token[l] != "" && mini_token[l] != "  "
                    && mini_token[l] != "   " && mini_token[l] != "    " && mini_token[l] != "\r" && mini_token[l] != "\r" + " " && mini_token[l] != "\n" + " "
                    && mini_token[l] != "        " && mini_token[l] != "\n " && mini_token[l] != "           ")


                {
                    naya_token.Add(mini_token[l]);
                }
            }

            foreach (string naya in naya_token)
            {
                richTextBox4.Text = richTextBox4.Text + '\n' + naya;
            }


            //token making
            string is_Key(string a1)
            {
                string val = "";
                if (Keywordss.TryGetValue(a1, out val))
                {
                    return val;
                }
                else
                {
                    return "no match";
                }
            }
            string is_Opr(string a2)
            {
                string val = "";
                if (Opr.TryGetValue(a2, out val))
                {
                    return val;
                }
                else
                {
                    return "no match";
                }
            }
            string is_Punc(string a3)
            {
                string val = "";
                if (Punct.TryGetValue(a3, out val))
                {
                    return val;
                }
                else
                {
                    return "no match";
                }
            }

            for (int z = 0; z < naya_token.Count; z++)
            {

                string ent = Convert.ToString(naya_token[z]);
                if (ent == "Enter")
                {
                    line_No++;
                    continue;
                }

                string kw = is_Key(Convert.ToString(naya_token[z]));
                if (kw != "no match")
                {
                    token obj = new token(kw, Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    kw = "";
                    continue;
                }

                string opr = is_Opr(Convert.ToString(naya_token[z]));
                if (opr != "no match")
                {
                    opr = is_Opr(Convert.ToString(naya_token[z]));
                    token obj = new token(opr, Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    opr = "";
                    continue;
                }

                string puncc = is_Punc(Convert.ToString(naya_token[z]));
                if (puncc != "no match")
                {
                    puncc = is_Punc(Convert.ToString(naya_token[z]));
                    token obj = new token(puncc, Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    puncc = "";
                    continue;
                }

                if (kw == "no match" && opr == "no match" && puncc == "no match")
                {

                    A1 = ID.IsMatch(Convert.ToString(naya_token[z]));
                    A2 = Int_Const.IsMatch(Convert.ToString(naya_token[z]));
                    A3 = Float_Const.IsMatch(Convert.ToString(naya_token[z]));
                    A4 = Char_Const.IsMatch(Convert.ToString(naya_token[z]));
                    A5 = Str_Const.IsMatch(Convert.ToString(naya_token[z]));

                }
                if (A1 == true)
                {
                    token obj = new token("ID", Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    A1 = false;
                    continue;
                }
                else if (A2 == true)
                {
                    token obj = new token("Int_Const", Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    A2 = false;
                    continue;
                }
                else if (A3 == true)
                {
                    token obj = new token("Flt_Const", Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);

                    A3 = false;
                    continue;
                }
                else if (A4 == true)
                {
                    token obj = new token("Char_Const", Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    A4 = false;
                    continue;
                }
                else if (A5 == true)
                {
                    token obj = new token("Str_Const", Convert.ToString(naya_token[z]), line_No);
                    tokenss.Add(obj);
                    A5 = false;

                    continue;
                }
                //else
                //{
                //    token obj = new token("Invalid", Convert.ToString(mini_token[i]), line_No);
                //    tokenss.Add(obj);
                //    A6 = false;

                //}
                if (A1 == false && A2 == false && A3 == false && A4 == false && A5 == false)
                {
                    token obj = new token("Invalid", Convert.ToString(naya_token[z]), line_No);
                    if (naya_token[z] != "\n" || naya_token[z] != " " || naya_token[z] != "  ")
                    {
                        tokenss.Add(obj);
                    }
                }
                A1 = false;
                A2 = false;
                A3 = false;
                A4 = false;
                A5 = false;


            }
            token obj1 = new token("$", "$", line_No);
            tokenss.Add(obj1);
            foreach (object s in tokenss)
            {
                richTextBox3.Text = richTextBox3.Text + '\n' + s.ToString();
            }


            //syntax analyzer

            token2[] TS;
            TS = new token2[tokenss.Count];
            for (int k = 0; k < tokenss.Count; k++)
            {
                string a1 = Convert.ToString(tokenss[k]);
                string[] parts = a1.Split('|');
                string Cp = parts[0].Substring(1).Trim();
                string Vp = parts[1].Trim();
                string LineNo = parts[2].Substring(0, parts[2].Length - 1);

                foreach (string xx in parts)
                {
                    TS[k] = new token2(Cp, Vp, LineNo);
                }

            }

            MessageBox.Show("Mapped All Tokens");
            for (int j = 0; j < TS.Length; j++)
            {
                richTextBox5.Text = richTextBox5.Text + TS[j].Get_CP() + TS[j].Get_VP() + TS[j].Get_Line();
            }
            //for(int i=0;i<TS.Length;i++)
            //{
            int i = 0;
            string CCC;
            CCC = TS[i].Get_CP();

            if (S1())
            {
                if (CCC == "$")
                {
                    MessageBox.Show("Valid Syntax");
                    richTextBox6.Text = "Valid Syntax";
                }
                

            }
            else if (S1() == false)
            {


                MessageBox.Show("Invalid Syntax");

                richTextBox6.Text = "Invalid Syntax" + " , " + "Error at Line" + TS[i].Get_Line();



            }



            bool S1()
            {
                if (Main())
                {
                    if (Classes())
                    {
                        return true;
                    }
                }
                return false;
            }

            bool Main()
            {
                if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP(); //Class part updated


                    if (CCC == "Class")
                    {
                        i++;
                        CCC = TS[i].Get_CP();

                        if (CCC == "MainClass")
                        {
                            i++;
                            CCC = TS[i].Get_CP();

                            if (ClassBase())
                            {
                                if (CCC == "{")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();

                                    if (CBODY())
                                    {
                                        if (CCC == "}")
                                        {
                                            i++;
                                            CCC = TS[i].Get_CP();
                                            return true;
                                        }

                                    }
                                }
                            }

                        }

                    }
                }
                return false;
            }
            bool ClassBase()
            {
                if (CCC == "::")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }
                }
                else if (CCC == "{")
                {
                    return true;
                }
                return false;
            }

            bool CBODY()
            {
                if (CBODYAttr())
                {
                    if (mainFunction())
                    {
                        if (CBODYAttr())
                        {
                            return true;
                        }
                    }
                }
                
                return false;
            }
            bool CBODYAttr()
            {
                if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Constr_Call1())
                    {
                        if (CCC == ";")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CBODYAttr())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (RT1())
                    {
                        if (CBODYAttr())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "void" || CCC == "}")
                {
                    return true;
                }
                return false;
            }
            bool mainFunction()
            {
                if (CCC == "void")
                {
                    i++;
                    CCC = TS[i].Get_CP();

                    if (CCC == "Main")
                    {
                        i++;
                        CCC = TS[i].Get_CP();

                        if (CCC == "(")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CCC == ")")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (Body())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Classes()
            {
                if (Class_Dec1())
                {
                    if (Classes())
                    {
                        return true;
                    }
                }
                else if (CCC == "$")
                {
                    return true;
                }
                return false;
            }
            bool Class_Dec1()
            {
                if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Class_Dec2())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool Class_Dec2()
            {
                if (AAA())
                {
                    if (ClassDec())
                    {
                        return true;
                    }
                }
                else if (Abstr_st())
                {
                    return true;
                }
                return false;
            }

            bool ClassDec()
            {
                if (CCC == "Class")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (ClassBase())
                        {
                            if (Body())
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            bool AAA()
            {
                if (CCC == "Sealed")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == "Class")
                {
                    return true;
                }
                return false;
            }
            bool Abstr_st()
            {
                if (CCC == "Abstract")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "Class")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (ClassBase())
                            {
                                if (CCC == "{")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();
                                    if (Abs_Functions())
                                    {
                                        if (CCC == "}")
                                            i++;
                                        CCC = TS[i].Get_CP();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Abs_Functions()
            {
                if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Abstr_AD())
                    {
                        if (Abs_N())
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
            bool Abstr_AD()
            {
                if (CCC == "Abstract")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (ART())
                    {
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CCC == "(")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (Func_Cond())
                                {
                                    if (CCC == ")")
                                        i++;
                                    CCC = TS[i].Get_CP();
                                    if (CCC == ";")
                                    {
                                        i++;
                                        CCC = TS[i].Get_CP();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Abs_N()
            {
                if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (functions())
                    {
                        if (Abs_N())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "}")
                {
                    return true;
                }
                return false;
            }
            bool functions()
            {
                if (Abstr_AD())
                {
                    return true;
                }
                else if (Func_st())
                {
                    return true;
                }
                return false;
            }

            bool ART()
            {
                if (CCC == "DT" || CCC == "void")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                return false;
            }

            bool Arr_Dec_E()
            {
                if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (MayBeExp())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool MayBeExp()
            {
                if (Exp())
                {
                    if (CCC == "]")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (Bracket())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "]")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "=")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CCC == "new")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (CCC == "ID")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();
                                    if (CCC == "[")
                                    {
                                        i++;
                                        CCC = TS[i].Get_CP();
                                        if (APL())
                                        {
                                            if (CCC == "]")
                                            {
                                                i++;
                                                CCC = TS[i].Get_CP();
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Bracket()
            {
                if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == "=")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Exp())
                    {
                        return true;
                    }
                }
                return false;
            }

            bool Body()
            {
                if (CCC == "{")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (MST())
                    {
                        if (CCC == "}")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }

                    }
                }
                return false;
            }

            bool MST()
            {

                if (SST())
                {
                    if (MST())
                    {
                        return true;
                    }
                }

                else if (CCC == "}" || CCC == "AM" || CCC == "Break")
                {

                    return true;
                }

                return false;
            }
            bool SST()
            {

                if (If_st())
                {
                    return true;
                }
                else if (Iff_st())
                {
                    return true;
                }
                else if (Floop_st())
                {
                    return true;
                }
                else if (Wlloop_st())
                {
                    return true;
                }
                else if (Break_st())
                {
                    return true;
                }
                else if (Jump_st())
                {
                    return true;
                }
                else if (Ret_st())
                {
                    return true;
                }
                else if (sw_st())
                {
                    return true;
                }
                else if (LocalDec())
                {
                    return true;
                }
                else if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (SST_2())
                    {
                        if (CCC == ";")
                            i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }
                }
                else if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (RT1())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool If_st()
            {
                if (CCC == "If")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "(")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (Exp())
                        {
                            if (CCC == ")")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (Body())
                                {
                                    if (Else_st())
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Else_st()
            {
                if (CCC == "Else")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Body())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool Iff_st()
            {
                if (CCC == "Iff")
                {
                    i++;
                    CCC = TS[i].Get_CP();

                    if (CCC == "(")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (Exp())
                        {
                            if (CCC == ")")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (Body())
                                {
                                    return true;
                                }
                            }
                        }
                    }

                }
                return false;
            }
            bool Jump_st()
            {
                if (CCC == "Jump")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == ";")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }
                return false;
            }
            bool Break_st()
            {
                if (CCC == "Break")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == ";")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }
                }
                return false;

            }

            bool sw_st()///////////////////
            {
                if (CCC == "Switch")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "(")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CCC == ")")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (CCC == "{")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();
                                    if (Case())
                                    {
                                        if (Default())
                                        {
                                            if (CCC == "}")
                                                i++;
                                            CCC = TS[i].Get_CP();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Case()
            {
                if (CCC == "Case")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Case_2())
                    {
                        if (CCC == ":")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (Body())
                            {
                                if (CCC == "Break")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();
                                    if (CCC == ";")
                                    {
                                        i++;
                                        CCC = TS[i].Get_CP();
                                        if (Case())
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (CCC == "Default")
                {
                    return true;
                }

                return false;
            }
            bool Case_2()
            {
                if (CCC == "Int_Const" || CCC == "Char_Const")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                return false;
            }

            bool Default()
            {
                if (CCC == "Default")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == ":")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (DFB())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            bool DFB()
            {
                if (CCC == "Break")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == ";")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }
                }
                else if (MST())
                {
                    if (CCC == "Break")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == ";")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }
                return false;
            }
            bool LIST()
            {
                if (CCC == ";")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == ",")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "DT")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (INIT())
                            {
                                if (LIST())
                                {
                                    return true;
                                }
                            }

                        }
                    }
                }
                return false;
            }
            bool INIT()
            {
                if (CCC == "=")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Exp())
                    {
                        return true;
                    }
                }
                else if (CCC == "," || CCC == ";")
                {
                    return true;
                }
                return false;
            }
            bool DecINIT()
            {
                if (CCC == "," || CCC == ";" || CCC == "=")
                {
                    if (INIT())
                    {
                        if (LIST())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            bool Dec()
            {
                if (CCC == "AM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "DT")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "ID")
                        {
                            if (DecINIT())
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            bool Wlloop_st()
            {
                if (CCC == "Wloop")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == ":")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "[")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (Exp())
                            {
                                if (CCC == "]")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();

                                    if (Body())
                                    {
                                        if (CCC == "EndWloop")
                                        {
                                            i++;
                                            CCC = TS[i].Get_CP();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                return false;
            }
            bool Floop_st()
            {
                if (CCC == "Floop")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == ":")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "[")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (C1())
                            {
                                if (C2())
                                {
                                    if (CCC == ";")
                                    {
                                        i++;
                                        CCC = TS[i].Get_CP();
                                        if (C3())
                                        {
                                            if (CCC == "]")
                                            {
                                                i++;
                                                CCC = TS[i].Get_CP();
                                                if (Body())
                                                {
                                                    if (CCC == "EndFloop")
                                                    {
                                                        i++;
                                                        CCC = TS[i].Get_CP();
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool C1()
            {
                if (LocalDec())
                {
                    return true;
                }
                else if (Asgn())
                {
                    if (CCC == ";")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }

                }
                else if (CCC == ";")
                {
                    return true;
                }
                return false;
            }
            bool C2()
            {
                if (Exp())
                {
                    return true;
                }
                else if (CCC == ";")
                {
                    return true;
                }
                return false;
            }
            bool C3()
            {
                if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (C33())
                    {
                        return true;
                    }
                }
                else if (CCC == "INC_DEC")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }
                }
                else if (CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool C33()
            {
                if (AS1())
                {
                    return true;
                }
                else if (CCC == "INC_DEC")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                return false;
            }
            bool Asgn()
            {
                if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (AS1())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool AS1()
            {
                if (Asgn_Op())
                {

                    if (Exp())
                    {

                        return true;
                    }
                }
                return false;
            }
            bool Asgn_Op()
            {
                if (CCC == "=" || CCC == "Asgn_Op")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;

                }
                return false;
            }
            bool Ret_st()
            {
                if (CCC == "return")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Exp())
                    {
                        if (CCC == ";")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }

                    }
                }
                return false;
            }

            bool Constr_Call1()
            {
                if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Call1_Constr())
                    {
                        return true;
                    }

                }
                return false;
            }
            bool Call1_Constr()
            {
                if (CCC == "=")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "new")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CCC == "(")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                if (Func_Cond())
                                {
                                    if (CCC == ")")
                                    {
                                        i++;
                                        CCC = TS[i].Get_CP();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (CCC == ";")
                {
                    return true;
                }
                return false;
            }
            bool APL()
            {
                if (CCC == "Int_Const" || CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool DecArrWala()
            {
                if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (APL())
                    {
                        if (CCC == "]")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (DecArrWala2())
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            bool DecArrWala2()
            {
                if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (APL())
                    {
                        if (CCC == "]")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }
                else if (CCC == ";" || CCC == "{" || CCC == "=")
                {
                    return true;
                }

                return false;
            }

            bool INIT_Arr()
            {
                if (CCC == ";")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == "=")
                {

                    i++;
                    CCC = TS[i].Get_CP();

                    if (CCC == "{")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (NewArray())
                        {

                            return true;
                        }
                    }
                }

                return false;
            }

            bool Arr_C()
            {
                if (Id_Const())
                {
                    if (Arr_C2())
                    {
                        if (CCC == "}")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (CCC == ";")
                            {
                                i++;
                                CCC = TS[i].Get_CP();
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            bool Arr_C2()
            {
                if (CCC == ",")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Id_Const())
                    {
                        if (Arr_C2())
                        {
                            return true;
                        }

                    }
                }
                else if (CCC == "}")
                {
                    return true;
                }
                return false;
            }
            bool Arr_C3()
            {
                if (CCC == "{")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Id_Const())
                    {
                        if (Arr_C2())
                        {
                            if (CCC == "}")
                            {
                                i++;
                                CCC = TS[i].Get_CP();

                                if (Arr_C4())
                                {

                                    if (CCC == "}")
                                    {
                                        i++;
                                        CCC = TS[i].Get_CP();
                                        if (CCC == ";")
                                        {
                                            i++;
                                            CCC = TS[i].Get_CP();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            bool Arr_C4()
            {
                if (CCC == ",")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "{")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (Id_Const())
                        {
                            if (Arr_C2())
                            {
                                if (CCC == "}")
                                {
                                    i++;
                                    CCC = TS[i].Get_CP();
                                    if (Arr_C4())
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (CCC == "}")
                {
                    return true;
                }
                return false;
            }
            bool NewArray()
            {
                if (CCC == "Int_Const" || CCC == "Flt_Const" || CCC == "Char_Const" || CCC == "Str_Const" || CCC == "Bool_Const" || CCC == "ID")
                {
                    if (Arr_C())
                    {
                        return true;
                    }
                }
                else if (CCC == "{")
                {

                    if (Arr_C3())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool Id_Const()
            {
                if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (Const())
                {
                    return true;
                }
                return false;
            }
            bool Const()
            {
                if (CCC == "Int_Const" || CCC == "Flt_Const" || CCC == "Char_Const" || CCC == "Str_Const" || CCC == "Bool_Const")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                return false;
            }

            bool X()
            {
                if (CCC == ",")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (E())
                    {
                        if (X())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (E())
                    {
                        if (CCC == "]")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (X())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == ")")
                {
                    return true;
                }
                return false;
            }

            bool FunctCall()
            {
                if (E())
                {
                    if (X())
                    {
                        return true;
                    }
                }
                return false;
            }

            bool Func_Call_Cond()
            {
                if (FunctCall())
                {
                    return true;
                }
                else if (CCC == ")")
                {
                    return true;
                }
                return false;
            }

            bool N_Func()
            {
                if (CCC == "(")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Func_Call_Cond())
                    {
                        if (CCC == ")")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }
                else if (ObjC())
                {
                    if (A66())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool ObjC()
            {

                if (CCC == ".")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (A44())
                        {
                            if (ObjC2())
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            bool ObjC2()
            {
                if (CCC == ".")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (A44())
                        {
                            if (ObjC2())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == "=" || CCC == ";")
                {
                    return true;
                }
                return false;
            }
            bool A44()
            {
                if (CCC == "[")
                {

                    i++;
                    CCC = TS[i].Get_CP();
                    if (E())
                    {
                        if (CCC == "]")
                        {
                            i++;
                            CCC = TS[i].Get_CP();

                            if (A55())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == "(")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Func_Call_Cond())
                    {
                        if (CCC == ")")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }

                else if (CCC == "INC_DEC")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == ";" || CCC == ".")
                {
                    return true;
                }
                return false;
            }
            bool A55()
            {
                if (CCC == "INC_DEC")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (CCC == ";" || CCC == ".")
                {
                    return true;
                }
                return false;
            }
            bool A66()
            {
                if (AS1())
                {
                    return true;
                }
                else if (CCC == ";")
                {
                    return true;
                }
                return false;
            }
            bool VO()
            {

                if (CCC == "VO" || CCC == "VO")
                {
                    i++;
                    CCC = TS[i].Get_CP();

                    return true;
                }
                else if (CCC == "(")
                {
                    return true;
                }
                return false;
            }
            bool Return_Type()
            {
                if (CCC == "DT" || CCC == "void")
                {
                    i++;
                    CCC = TS[i].Get_CP();

                    return true;
                }
                else if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        return true;

                    }
                    if (VO())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool Func_st()
            {
                if (Return_Type())
                {
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (VO())
                        {
                            if (DecFuncCond())
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            bool DecFuncCond()
            {
                if (CCC == "(")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (Func_Cond())
                    {
                        if (CCC == ")")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (Body())
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            bool FC2()
            {
                if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "]")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (FCB2())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "," || CCC == ")")
                {
                    return true;
                }
                return false;
            }
            bool FCB2()
            {
                if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "]")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }
                }
                else if (CCC == "," || CCC == ")")
                {
                    return true;
                }
                return false;
            }
            bool Func_Cond2()
            {
                if (CCC == ",")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "DT")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (FC2())
                            {
                                if (Func_Cond2())
                                {
                                    return true;
                                }
                            }
                        }
                    }

                }
                else if (CCC == ")")
                {
                    return true;
                }
                return false;
            }

            bool Func_Cond()
            {
                if (CCC == "DT")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (FC2())
                        {
                            if (Func_Cond2())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == ")")
                {
                    return true;
                }
                return false;
            }

            bool SST_2()
            {
                if (N_Func())
                {

                    return true;
                }
                else if (AS1())
                {
                    return true;
                }
                else if (CCC == "INC_DEC")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    return true;
                }
                else if (Constr_Call1())
                {
                    return true;
                }
                else if (Arr_Dec_E())
                {
                    return true;
                }
                return false;
            }

            bool RT1()
            {
                if (CCC == "DT")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (ArrayHoSakta())
                    {
                        if (CCC == "ID")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (RT2())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (RT3())
                    {
                        return true;
                    }
                }
                else if (CCC == "void")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (VO())
                        {
                            if (DecFuncCond())
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            bool CanBeMain()
            {
                if (CCC == "Main")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "(")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == ")")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            if (Body())
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (VO())
                    {
                        if (DecFuncCond())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            bool RT2()
            {

                if (DecArrWala())
                {
                    if (INIT_Arr())
                    {
                        return true;
                    }
                }
                else if (DecINIT())
                {
                    return true;
                }
                else if (VO())
                {

                    if (DecFuncCond())
                    {
                        return true;
                    }
                }
                return false;
            }

            bool RT3()
            {
                if (DecFuncCond())
                {
                    return true;
                }
                else if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (VO())
                    {
                        if (DecFuncCond())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }



            bool Exp()
            {
                if (O())
                {
                    return true;
                }
                return false;
            }

            bool O()
            {
                if (A())
                {
                    if (O_Dash())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool O_Dash()
            {
                if (CCC == "||")
                {
                    if (A())
                    {
                        if (O_Dash())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "," || CCC == ";" || CCC == ")" || CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool A()
            {
                if (R())
                {
                    if (A_Dash())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool A_Dash()
            {
                if (CCC == "&&")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (R())
                    {
                        if (A_Dash())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "||" ||
                         CCC == "," || CCC == ";" || CCC == ")" || CCC == "]")
                {
                    return true;
                }

                return false;
            }
            bool R()
            {
                if (E())
                {
                    if (R_Dash())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool R_Dash()
            {
                if (CCC == "ROP")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (E())
                    {
                        if (R_Dash())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "&&" || CCC == "||" ||
                         CCC == "," || CCC == ";" || CCC == ")" || CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool E()
            {
                if (T())
                {
                    if (E_Dash())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool E_Dash()
            {
                if (CCC == "PM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (T())
                    {
                        if (E_Dash())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "ROP" || CCC == "&&" || CCC == "||" ||
                         CCC == "," || CCC == ";" || CCC == ")" || CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool T()
            {
                if (F())
                {
                    if (T_Dash())
                    {
                        return true;
                    }
                }
                return false;
            }
            bool T_Dash()
            {
                if (CCC == "MDM")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (F())
                    {
                        if (T_Dash())
                        {
                            return true;
                        }
                    }
                }
                else if (CCC == "PM" || CCC == "ROP" || CCC == "&&" ||
                    CCC == "||" || CCC == "," || CCC == ";" || CCC == ")" || CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool F()
            {
                if (Const())
                {
                    return true;
                }
                else if (CCC == "(")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (O())
                    {
                        if (CCC == ")")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }
                else if (CCC == "!")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (F())
                    {
                        return true;
                    }
                }
                else if (CCC == "INC_DEC")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (CCC == ";")
                        {
                            i++;
                            CCC = TS[i].Get_CP();
                            return true;
                        }
                    }
                }
                else if (CCC == "ID")
                {
                    i++;
                    CCC = TS[i].Get_CP();

                    if (F_Dash())
                    {

                        return true;
                    }
                }
                return false;
            }

            bool F_Dash()
            {
                if (SST_2())
                {
                    return true;
                }
                else if (CCC == "MDM" || CCC == "PM" ||
                   CCC == "ROP" || CCC == "&&" || CCC == "||" ||
                   CCC == "," || CCC == ";" || CCC == ")" || CCC == "]")
                {
                    return true;
                }
                return false;
            }
            bool ArrayHoSakta()
            {
                if (CCC == "[")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "]")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        return true;
                    }

                }
                else if (CCC == "If" || CCC == "Iff" || CCC == "Floop"
                || CCC == "Wloop" || CCC == "Switch" || CCC == "Break"
                || CCC == "Jump" || CCC == "return" || CCC == "ID" || CCC == "AM")
                {
                    return true;
                }
                return false;
            }

            bool LocalDec()
            {
                if (CCC == "DT")
                {
                    i++;
                    CCC = TS[i].Get_CP();
                    if (CCC == "ID")
                    {
                        i++;
                        CCC = TS[i].Get_CP();
                        if (DecINIT())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }




            // }


        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
           
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        public class token
        {
            string class_part;
            string value_part;
            int line_no;

           


            public token(string cp, string vp, int ln)
            {
                class_part = cp;
                value_part = vp;
                line_no = ln;
                
            }

            public override string ToString()
            {
                return "(" + class_part + ' ' + '|' + ' ' + value_part + ' ' + '|' + ' ' + line_no + ")";
            }


        }
        public class token2
        {
            string class_part;
            string value_part;
            string line_no;




            public token2(string cp, string vp, string ln)
            {
                class_part = cp;
                value_part = vp;
                line_no = ln;
            }

            public override string ToString()
            {
                return "(" + class_part + ' ' + '|' + ' ' + value_part + ' ' + '|' + ' ' + line_no + ")";
            }

            public string Get_CP()
            {
                return class_part;
            }
            public string Get_VP()
            {
                return value_part;
            }
            public string Get_Line()
            {
                return line_no;
            }
        }

        //Symantec
        public class ClassTable
        {
            public string name;
            public string type;
            public string cat;
            public string parent;
            ClassAttributes reference;

            public ClassTable()
            {
                name = "";
                type = "";
                cat = "";
                parent = "";
                reference = null;
            }

            ClassTable(string n, string t, string c, string p, ClassAttributes r)
            {
                name = n;
                type = t;
                cat = c;
                parent = p;
                reference = r;
            }
            public List<ClassTable> cTable = new List<ClassTable>();
          
            
            public bool Insert(string n, string t, string c, string p , ClassAttributes r)
            {
                ClassTable ct1 = new ClassTable(n, t, c, p, r);

                if (!cTable.Contains(ct1))
                {
                    cTable.Add(new ClassTable(n, t, c, p, r));
                    return true;
                }
                return false;
            }

            
            public string lookup(string n)
            {
                string type = " ";
                foreach(ClassTable ct1 in cTable)
                {
                    if(ct1.name == n)
                    {
                        type = ct1.type;
                    }
                }
                return type;
            }
          
        }
       public class ClassAttributes
        {
            public string name;
            public string type;
            public string AM;
            public string AT;
            ClassAttributes refOfCT;

            public ClassAttributes()
            {
                name = "";
                type = "";
                AM = "";
                AT = "";
                refOfCT = null;
            }

            public ClassAttributes(string n, string t, string a, string at, ClassAttributes r)
            {
                name = n;
                type = t;
                AM = a;
                AT = at;
                refOfCT = r;
            }
           public List<ClassAttributes> dataTable = new List<ClassAttributes>();

            public bool Insert_CT(string n, string t, string a, string at, ref ClassAttributes r)
            {
                ClassAttributes ca = new ClassAttributes(n, t, a, at, r);

                if(!dataTable.Contains(ca))
                {
                    dataTable.Add(new ClassAttributes(n, t, a, at, r));

                    return true;
                }
                return false;
            }
           
            //public string Lookup_CT(string n, out string t , out string a, out string at)
            //{
                
            //}

        }

        public class FunctionData
        {
            public string name;
            public string type;
            public int scope;


            public FunctionData()
            {
                name = "";
                type = "";
                scope = 0;

            }

            public FunctionData(string n, string t, int s)
            {
                name = n;
                type = t;
                scope = s;
            }
           public  List<FunctionData> fData = new List<FunctionData>();

            public bool Insert_FT(string n, string t, int s)
            {
                FunctionData fd = new FunctionData(n,t,s);

                if(!fData.Contains(fd))
                {
                    fData.Add(new FunctionData(n, t, s));
                    return true;
                }
                return false;
            }
        }

        public class Compatibility
        {

            string compatibilityCheck(string T1, string T2, string Op)
            {
                if (Op == "+" || Op == "-" || Op == "*" || Op == "/" || Op == "=" || Op == "||" || Op == "&&" || Op == "+=" || Op == "-=" ||
                    Op == "*=" || Op == "/=" || Op == ">" || Op == "<" || Op == "==" || Op == ">=" || Op == "<=" || Op == "%" || Op == "%=" ||
                    Op == "!" || Op == "!=")
                {

                    if (T1 == "int" && T2 == "int")
                    {
                        MessageBox.Show("Compatibility is checked. Plz Continue..");
                    }
                    else if (T1 == "float" && T2 == "float")
                    {
                        MessageBox.Show("Compatibility is checked. Plz Continue..");
                    }
                    else if (T1 == "char" && T2 == "char")
                    {
                        MessageBox.Show("Compatibility is checked. Plz Continue..");
                    }
                    else if (T1 == "string" && T2 == "string")
                    {
                        MessageBox.Show("Compatibility is checked. Plz Continue..");
                    }
                    else
                        MessageBox.Show("Mismatch Error");
                }

                return "";
            }
        }

        public class ScopeDeal
        {
            int scope = 0;

            Stack<int> myStack = new Stack<int>();

            int CreateScope()
            {
                scope++;
                myStack.Push(scope);
                return scope;
            }

            int DestroyScope()
            {
                myStack.Pop();
                scope--;
                return scope;
            }
        }
        
        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClassTable ct = new ClassTable();
            ClassAttributes ca = new ClassAttributes();
            FunctionData fd = new FunctionData();
            
            void DisplayClass()
            {

                richTextBox7.Text = "NAME |\tTYPE |\tCATEGORY |\tPARENT\n";
                for(int l=0;l<ct.cTable.Count;l++)
                {
                    richTextBox7.Text = richTextBox7.Text + ct.cTable[l].name + "|\t" + ct.cTable[l].type + "|\t" + ct.cTable[l].cat + "|\t" 
                    + ct.cTable[l].parent ;
                }
            }

            void DisplayClassData()
            {
                richTextBox8.Text = "NAME |\tTYPE |\tAM |\tAT \n";
                for(int l=0;l<ca.dataTable.Count;l++)
                {
                    richTextBox8.Text = richTextBox8.Text + ca.dataTable[l].name + "|\t" + ca.dataTable[l].type + "|\t" + ca.dataTable[l].AM
                    + "|\t" + ca.dataTable[l].AT ;
                }
            }

            void DisplayFunctionData()
            {
                richTextBox9.Text = "NAME |\tTYPE |\tSCOPE \n";
                for(int l=0;l<fd.fData.Count;l++)
                {
                    richTextBox9.Text = richTextBox9.Text + fd.fData[l].name + "|\t" + fd.fData[l].type + "|\t" + fd.fData[l].scope;
                }
            }
        }
    }
}

