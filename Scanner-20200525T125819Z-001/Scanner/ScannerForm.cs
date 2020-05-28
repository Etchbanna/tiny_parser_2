using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanner
{
 
    public partial class ScannerForm : Form

    {
        TreeNode ifstmt;
        TreeNode operator_=new TreeNode("");
        TreeNode factor_ = new TreeNode("");
        TreeNode assignstmt = new TreeNode("");
        TreeNode repeatstmt = new TreeNode("");
        TreeNode writestmt = new TreeNode("");
        public enum TokenType{
            IDENTIFIER,
            NUMBER
}
        public class Token{
            TokenType type;
            public Token(string s){

}

}
       
        string code;
        string code2;
        string tokenindicator = "$";
        string CurrentToken="";
        public ScannerForm()
        {
            InitializeComponent();
            TokensLabel.Text = "";
            TokensLabel.Hide();

        }
        
        bool isDigit(char a) {
            return (a >= '0' && a <= '9');
        }

        
        bool isLetter(char l)
        { return (l >= 'a' && l <= 'z' || l >= 'A' && l <= 'Z');
        }

       
        bool isSymbol(char c)
        {
            return (c == '+' || c == '-' || c == '*' || c== '–' || c==':' || c == '=' || c == '<' || c == ';' || c == '>' || c=='/' ||
                c == '{' || c == '}' || c == ',' || c == '"' || c == '(' || c == ')' || c=='|' || c=='&'
                );
        }
        
        bool isSpace(char s) { return (s == ' ' || s == '\t' || s == '\n'); }
         
        
        public enum nameofState { START, COMMENTSTATEMENT, NUMBER, IDENTIFIER, ASSIGNMENT, RESERVEDWORD, IDENTIFIERorRESERVEDWORD ,OPERATOR,END };
        nameofState state = nameofState.START;
        
        

        //reserved words
        string[] RESERVERDWORD = { "if", "then", "else", "end", "repeat", "until", "read", "write", "int", "float", "string" , "return", "elseif"};


        private void ShowButtonClicked(object sender, EventArgs e)
        {
            code = LexemeTextBox.Text;
            getToken(code+=" ");
            TokensLabel.Show();

        }

        private void TextboxClicked(object sender, EventArgs e)
        {

        }
        private void LabelClicked(object sender, EventArgs e)
        {

        }



        public void getToken(string s)
        {
            string mytoken="";
            bool res_flag = false;
            int i = 0;
            while (state != nameofState.END)
            {
                switch (state)
                {
                    case nameofState.START:
                        if (isSpace(s[i]))
                        {
                            i++;
                            if (i == s.Length) state = nameofState.END;
                            else state = nameofState.START;
                        }
                        else if (isDigit(s[i]))
                        {
                            state = nameofState.NUMBER;
                        }
                        else if (isLetter(s[i]))
                        {
                            state = nameofState.IDENTIFIER;
                        }
                        else if (s[i] == ':')
                        {
                            state = nameofState.ASSIGNMENT;
                        }
                        else if (s[i] == '/' && s[i+1]=='*')
                        {
                            
                            state = nameofState.COMMENTSTATEMENT;
                        }
                        else if (isSymbol(s[i]))
                        {
                            switch (s[i])
                            {
                                case ';': TokensLabel.Text += tokenindicator+ s[i]+"\n"; break;
                                case '"':
                                    mytoken += s[i];
                                    do
                                    {
                                        i++;
                                        mytoken += s[i];

                                    }
                                    while (s[i] != '"');
                                    i++;
                                    TokensLabel.Text += tokenindicator+ mytoken + " ---- string \n";
                                    mytoken = "";
                                    break;
                                case '|':
                                case '&':
                                    mytoken += s[i];
                                    i++;
                                    mytoken += s[i];
                                    i++;
                                    TokensLabel.Text += tokenindicator + mytoken + " ---- boolean_operator \n";
                                    mytoken = "";
                                    break;
                                case '+':
                                case '-':
                                case '*':
                                case '/':
                                case '–':
                                    mytoken += s[i];
                                    TokensLabel.Text += tokenindicator + mytoken + " ---- arithmetic operator \n";
                                    mytoken = "";
                                    break;
                                case '>':
                                case '<':
                                case '=':
                                    mytoken += s[i];
                                    
                                    TokensLabel.Text += tokenindicator + mytoken + " ---- condition operator \n";
                                    mytoken = "";
                                    break;
                                



                                default: TokensLabel.Text += s[i]+" ---- symbol \n"; break;
                            }
                            i++;
                            if (i == s.Length) state = nameofState.END;
                            else state = nameofState.START;
                        }
                        else state = nameofState.END;
                        break;
                    case nameofState.COMMENTSTATEMENT:
                        if (state == nameofState.COMMENTSTATEMENT)
                        {
                            mytoken += s[i];
                            i++;
                            while (s[i] != '/')
                            {
                                mytoken += s[i];
                                i++;
                            }
                            mytoken += s[i];
                            TokensLabel.Text+= tokenindicator + mytoken + " ---- comment statement \n" ;
                            i++;
                            mytoken = "";
                            if (i == s.Length) state = nameofState.END;
                            else state = nameofState.START;
                        }
                        break;

                    case nameofState.NUMBER:
                        while (isDigit(s[i]) || s[i]=='.')
                        {
                            mytoken += s[i];
                            i++;
                        }
                        TokensLabel.Text+= tokenindicator + mytoken +" ---- number \n";
                        mytoken = "";
                        if (i == s.Length) state = nameofState.END;
                        else state = nameofState.START;
                        break;

                    case nameofState.IDENTIFIER:
                        while (isLetter(s[i]) || isDigit(s[i]) )
                        {
                                mytoken += s[i];
                                i++;
                        }
                        for (int count = 0; count < 13; count++)
                        {
                            if (RESERVERDWORD[count] == mytoken) {
                                res_flag = true;
                                switch (RESERVERDWORD[count])
                                {
                                    case "return":
                                        mytoken += s[i];
                                        while (s[i] != ';')
                                        {
                                            i++;
                                            mytoken += s[i];
                                        }
                                        TokensLabel.Text += tokenindicator + mytoken + " ---- return statement \n";
                                        break;
                                    case "int":
                                    case "float":
                                    case "string":
                                        TokensLabel.Text += tokenindicator + mytoken + " ---- reserved word(datatype) \n";
                                        break;
                                    case "if":
                                        TokensLabel.Text += tokenindicator + mytoken + " ---- reserved word(if statement) \n";
                                        break;
                                    case "write":
                                        mytoken += s[i];
                                       
                                        TokensLabel.Text += tokenindicator + mytoken + " ---- write statement \n";
                                        break;
                                    case "read":
                                        mytoken += s[i];
                                        

                                        TokensLabel.Text += tokenindicator+ mytoken + " ---- read statement \n";
                                        break;
                                    case "repeat":
                                        TokensLabel.Text += tokenindicator + mytoken + " ---- reserved word(repeat statement) \n";
                                        break;
                                    default:
                                        TokensLabel.Text += tokenindicator + mytoken + " ---- reserved word \n";
                                        break;


                                }
                            };
                        }
                        
                        if (s[i] == '(')
                        {
                            mytoken += s[i];
                            do
                            {
                                i++;
                                mytoken += s[i];

                            }
                            while (s[i] != ')');
                            i++;

                            TokensLabel.Text += tokenindicator + mytoken + " ---- function call \n";
                        }
                        else if(res_flag==false) TokensLabel.Text += tokenindicator + mytoken + " ---- identifier \n";
                        mytoken = "";
                        res_flag = false;
                        if (i == s.Length) state = nameofState.END;
                        else state = nameofState.START;
                        break;

                    case nameofState.ASSIGNMENT:
                        if (s[i] == ':')
                        {
                            
                            TokensLabel.Text += tokenindicator + ":= ---- assignment operator \n";
                            i+=2;
                            
                            state = nameofState.START;
                        }
                        else
                        {
                            if (i == s.Length) state = nameofState.END;
                            else state = nameofState.START;
                        }
                        break;
                    case nameofState.END:
                        break;

                }


            }
        }
        int startFromHere=1;
        public void getNexttokenParser(string s, ref string CurrentToken, ref int startFromHere, ref nameofState state){
            state=nameofState.START;
             if (startFromHere == s.Length)
                state = nameofState.END;

            else state = nameofState.START;
            CurrentToken="";
            bool res_flag = false;
            while (state != nameofState.END)
            {
                switch (state)
                {
                    case nameofState.START:
                        if (isSpace(s[startFromHere])) {
                            
                            while (s[startFromHere] != '$' )
                            {


                                startFromHere++;

                                if (startFromHere == s.Length)
                                {
                                    state = nameofState.END;
                                    break;
                                }

                                else state = nameofState.START;
                                }
                            startFromHere++;
                            
                            
                            }
                        
                        else if (isDigit(s[startFromHere]))
                        {
                            state = nameofState.NUMBER;
                        }
                        else if (isLetter(s[startFromHere]))
                        {
                            state = nameofState.IDENTIFIERorRESERVEDWORD;
                        }
                        else if (s[startFromHere] == ':')
                        {
                            state = nameofState.ASSIGNMENT;
                        }
                        else if (s[startFromHere] == '/' && s[startFromHere + 1] == '*')
                        {

                            state = nameofState.COMMENTSTATEMENT;
                        }
                        else if (isSymbol(s[startFromHere]))
                        {
                            switch (s[startFromHere])
                            {
                                case '(':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    return;

                                case ')':

                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    return;


                                case ';':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    return;

                                case '"':
                                    CurrentToken += s[startFromHere];
                                    do
                                    {
                                        startFromHere++;
                                        CurrentToken += s[startFromHere];

                                    }
                                    while (s[startFromHere] != '"');
                                    startFromHere++;

                                    return;


                                case '|':
                                case '&':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    return;


                                case '+':
                                case '-':
                                case '–':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    state = nameofState.OPERATOR;
                                    return;
                                case '*':
                                case '/':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    state = nameofState.OPERATOR;
                                    return;


                                case '>':
                                case '<':
                                case '=':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    state = nameofState.OPERATOR;
                                    return;
                                case ':':
                                    CurrentToken += s[startFromHere];
                                    startFromHere++;
                                    return;



                                default:
                                    break;
                            }
                            startFromHere++;
                            if (startFromHere == s.Length) state = nameofState.END;
                            else state = nameofState.START;
                        }
                        else state = nameofState.END;
                        break;
                    case nameofState.COMMENTSTATEMENT:
                        if (state == nameofState.COMMENTSTATEMENT)
                        {
                            CurrentToken += s[startFromHere];
                            startFromHere++;
                            while (s[startFromHere] != '/')
                            {
                                CurrentToken += s[startFromHere];
                                startFromHere++;
                            }
                            CurrentToken += s[startFromHere];
                            
                            startFromHere++;
                            return;
                            
                            
                        }
                        break;

                    case nameofState.NUMBER:
                        while (isDigit(s[startFromHere]) || s[startFromHere]=='.')
                        {
                            CurrentToken += s[startFromHere];
                            startFromHere++;
                        }
                        
                        return;
                        

                    case nameofState.IDENTIFIERorRESERVEDWORD:
                        while (isLetter(s[startFromHere]) || isDigit(s[startFromHere]) )
                        {
                                CurrentToken += s[startFromHere];
                                startFromHere++;
                        }
                        for (int count = 0; count < 13; count++)
                        {
                            if (RESERVERDWORD[count] == CurrentToken) {
                                state=nameofState.RESERVEDWORD;
                                res_flag = true;
                                switch (RESERVERDWORD[count])
                                {
                                    case "return":
                                        CurrentToken += s[startFromHere];
                                        while (s[startFromHere] != ';')
                                        {
                                            startFromHere++;
                                            CurrentToken += s[startFromHere];
                                        }
                                        
                                        startFromHere++;
                                        return;
                                        
                                    case "int":
                                    case "float":
                                    case "string":
                                        TokensLabel.Text += CurrentToken + " ---- reserved word(datatype) \n";
                                        return;
                                       
                                    case "if":
                                        
                                        return;
                                        
                                    case "write":
                                        
                                       
                                        
                                        return;
                                       
                                    case "read":

                                        
                                        

                                        
                                        return;
                                       
                                    case "repeat":
                                       
                                        return;
                                    case "end":
                                        return;
                                       
                                    default:
                                        
                                        return;
                                 


                                }
                            };
                        }
                        
                        if (s[startFromHere] == '(')
                        {
                            CurrentToken += s[startFromHere];
                            do
                            {
                                startFromHere++;
                                CurrentToken += s[startFromHere];

                            }
                            while (s[startFromHere] != ')');
                            startFromHere++;
                            return;

                            
                        }
                        else if(res_flag==false){

                             
                           state=nameofState.IDENTIFIER;
                           return;

}
                        CurrentToken = "";
                        res_flag = false;
                        if (startFromHere == s.Length) state = nameofState.END;
                        else state = nameofState.START;
                        break;

                    case nameofState.ASSIGNMENT:
                        if (s[startFromHere] == ':')
                        {
                            CurrentToken+=s[startFromHere];
                            startFromHere++;
                            CurrentToken += s[startFromHere];
                            startFromHere++;

                            return;
                           
                          
                        }
                        else if(s[startFromHere]=='='){
                            CurrentToken+=s[startFromHere];
                            startFromHere++;
                            return;
}
                        else
                        {
                            if (startFromHere == s.Length) state = nameofState.END;
                            else state = nameofState.START;
                        }
                        break;
                    case nameofState.END:
                        break;

                }

}}
        public void ParserLaunch()
        {
            
            program();
        }
        
        public void match(string input, string expected){
            if (input == expected)
            {
                if (input == ";")
                {
                    ParseTextBox.Text += "" + CurrentToken + "             " + "\n";
                    getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
                }
                else
                {



                    ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";


                    getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
                }
                


            }
            else
                ParseTextBox.Text ="Syntax Error";
}
        
        public void program()
        {
            
            code2 = TokensLabel.Text+" ";
            getNexttokenParser(code2,ref CurrentToken, ref startFromHere, ref state);
            stmt_sequence();
        }
        public void stmt_sequence(){
            statement();
            while( CurrentToken == ";"){
                match(CurrentToken,";");
                statement();
}
}
        public void stmt_sequenceFromRepeatStmt()
        {
            statementFromRepeatStmt();
            while (CurrentToken == ";")
            {
                match(CurrentToken, ";");
                statementFromRepeatStmt();
            }
        }
        public void stmt_sequenceFromIfStmt()
        {
            statementFromIfStmt();
            while (CurrentToken == ";")
            {
                match(CurrentToken, ";");
                statementFromIfStmt();
            }
        }
        public void statement(){
            if (CurrentToken == "if")
                if_stmt();
            else if( CurrentToken =="repeat")
                repeat_stmt();
            else if (state ==nameofState.IDENTIFIER)
                assign_stmt();
            else if(CurrentToken=="read")
                read_stmt();
            else if(CurrentToken=="write")
                write_stmt();
            else 
                ParseTextBox.Text+="Syntax Error";
}
        public void statementFromRepeatStmt()
        {
            if (CurrentToken == "if")
                if_stmtFromRepeatStmt();
            else if (CurrentToken == "repeat")
                repeat_stmt();
            else if (state == nameofState.IDENTIFIER)
                assign_stmtFromRepeatStmt();
            else if (CurrentToken == "read")
                read_stmtFromRepeatStmt();
            else if (CurrentToken == "write")
                write_stmtFromRepeatStmt();
            else
                ParseTextBox.Text += "Syntax Error";
        }
        public void statementFromIfStmt()
        {
            if (CurrentToken == "if")
                if_stmt();
            else if (CurrentToken == "repeat")
                repeat_stmtFromIfStmt();
            else if (state == nameofState.IDENTIFIER)
                assign_stmtFromIfStmt();
            else if (CurrentToken == "read")
                read_stmtFromIfStmt();
            else if (CurrentToken == "write")
                write_stmtFromIfStmt();
            else
                ParseTextBox.Text += "Syntax Error";
        }

        public void if_stmt(){
            ifstmt = new TreeNode("if");
            
            
            SyntaxTree.Nodes.Add(ifstmt);
            
            match(CurrentToken,"if");
            expFromIfStmt();
            match(CurrentToken,"then");
            stmt_sequenceFromIfStmt();

            if (CurrentToken=="else"){
                match(CurrentToken,"else");
                stmt_sequence();
}

            match(CurrentToken,"end");
}
        public void if_stmtFromIfStmt()
        {
            ifstmt = new TreeNode("if");


            ifstmt.Nodes.Add(ifstmt);

            match(CurrentToken, "if");
            expFromIfStmt();
            match(CurrentToken, "then");
            stmt_sequenceFromIfStmt();

            if (CurrentToken == "else")
            {
                match(CurrentToken, "else");
                stmt_sequence();
            }

            match(CurrentToken, "end");
        }
        public void if_stmtFromRepeatStmt()
        {
            ifstmt = new TreeNode("if");


            repeatstmt.Nodes.Add(ifstmt);

            match(CurrentToken, "if");
            expFromIfStmt();
            match(CurrentToken, "then");
            stmt_sequenceFromRepeatStmt();

            if (CurrentToken == "else")
            {
                match(CurrentToken, "else");
                stmt_sequenceFromRepeatStmt();
            }

            match(CurrentToken, "end");
        }
        public void repeat_stmt(){
            repeatstmt = new TreeNode("Repeat");
            SyntaxTree.Nodes.Add(repeatstmt);
            match(CurrentToken,"repeat");
            stmt_sequenceFromRepeatStmt();
            match(CurrentToken,"until");
            expFromRepeatStmt();
}
        public void repeat_stmtFromIfStmt()
        {
            repeatstmt = new TreeNode("Repeat");
            ifstmt.Nodes.Add(repeatstmt);
            match(CurrentToken, "repeat");
            stmt_sequenceFromRepeatStmt();
            match(CurrentToken, "until");
            expFromRepeatStmt();
        }
        public void assign_stmt(){
            ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
            assignstmt=new TreeNode("Assign (" + CurrentToken + ")");
            SyntaxTree.Nodes.Add(assignstmt);

            getNexttokenParser(code2,ref CurrentToken, ref startFromHere, ref state);
            match(CurrentToken,":=");
            
            expFromAssignStmt();

}
        public void assign_stmtFromRepeatStmt()
        {
            ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
            assignstmt = new TreeNode("Assign (" + CurrentToken + ")");
            repeatstmt.Nodes.Add(assignstmt);

            getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
            match(CurrentToken, ":=");

            expFromAssignStmt();

        }
        public void assign_stmtFromIfStmt()
        {
            ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
            assignstmt = new TreeNode("Assign (" + CurrentToken + ")");
            ifstmt.Nodes.Add(assignstmt);

            getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
            match(CurrentToken, ":=");

            expFromAssignStmt();

        }
        public void read_stmt(){
            
            match(CurrentToken,"read");
            TreeNode readstmt = new TreeNode("read (" + CurrentToken + ")");
            SyntaxTree.Nodes.Add(readstmt);
            
            

            match("IDENTIFIER", state.ToString());
            
}
        public void read_stmtFromRepeatStmt()
        {

            match(CurrentToken, "read");
            TreeNode readstmt = new TreeNode("read (" + CurrentToken + ")");
            repeatstmt.Nodes.Add(readstmt);



            match("IDENTIFIER", state.ToString());

        }
        public void read_stmtFromIfStmt()
        {

            match(CurrentToken, "read");
            TreeNode readstmt = new TreeNode("read (" + CurrentToken + ")");
            ifstmt.Nodes.Add(readstmt);



            match("IDENTIFIER", state.ToString());

        }
        public void write_stmt(){

            match(CurrentToken,"write");
            writestmt = new TreeNode("write");
            SyntaxTree.Nodes.Add(writestmt);
            expFromWriteStmt();
}
        public void write_stmtFromRepeatStmt()
        {
            match(CurrentToken, "write");
            expFromRepeatStmt();
        }
        public void write_stmtFromIfStmt()
        {

            match(CurrentToken, "write");
            writestmt = new TreeNode("write");
            ifstmt.Nodes.Add(writestmt);
            expFromWriteStmt();
        }
        public void exp()
        {
            simple_exp();
            if (CurrentToken == "<" || CurrentToken == "=")
            {
                comparison_op();
                simple_exp();
            }
        }

        public void expFromAssignStmt()

        {
                simple_expFromAssignStmt();
                if (CurrentToken == "<" || CurrentToken == "=")
                {
                    comparison_op();
                    simple_expFromAssignStmt();
                }
            }
        public void expFromIfStmt()
        {
            simple_expFromIfStmt();
            if (CurrentToken == "<" || CurrentToken == "=")
            {
                comparison_op();
                simple_expFromIfStmt();
            }
        }
        public void expFromWriteStmt()
        {
            simple_expFromWriteStmt();
            if (CurrentToken == "<" || CurrentToken == "=")
            {
                comparison_op();
                simple_exp();
            }
        }
        public void expFromRepeatStmt()
        {
            simple_expFromRepeatStmt();
            if (CurrentToken == "<" || CurrentToken == "=")
            {
                comparison_opFromRepeatStmt();
                simple_expFromRepeatStmt();
            }
        }
        public void comparison_op(){


            operator_ = new TreeNode();
            operator_.Text= ("Operator (" + CurrentToken + ")");
            ifstmt.Nodes.Add(operator_);
       
            if (CurrentToken=="<")
                match(CurrentToken,"<");
            else if(CurrentToken=="=")
                match(CurrentToken,"=");
}
        public void comparison_opFromAssignStmt()
        {


            operator_ = new TreeNode();
            operator_.Text = ("Operator (" + CurrentToken + ")");
            ifstmt.Nodes.Add(operator_);

            if (CurrentToken == "<")
                match(CurrentToken, "<");
            else if (CurrentToken == "=")
                match(CurrentToken, "=");
        }
        public void comparison_opFromRepeatStmt()
        {


            operator_ = new TreeNode();
            operator_.Text = ("Operator (" + CurrentToken + ")");
            repeatstmt.Nodes.Add(operator_);

            if (CurrentToken == "<")
                match(CurrentToken, "<");
            else if (CurrentToken == "=")
                match(CurrentToken, "=");
        }
        public void simple_exp(){
            term();
            while( CurrentToken == "+" || CurrentToken =="-" || CurrentToken== "–"){
                addop();
                term();
}

}
        public void simple_expFromAssignStmt()
        {
            termFromAssignStmt();
            while (CurrentToken == "+" || CurrentToken == "-" || CurrentToken == "–")
            {
                addopFromAssignStmt();
                termFromAssignStmt();
            }
        }
        public void simple_expFromIfStmt()
        {
            termFromIfStmt();
            while (CurrentToken == "+" || CurrentToken == "-" || CurrentToken == "–")
            {
                addop();
                term();
            }

        }
        public void simple_expFromWriteStmt()
        {
            termFromWriteStmt();
            while (CurrentToken == "+" || CurrentToken == "-" || CurrentToken == "–")
            {
                addop();
                term();
            }

        }
        public void simple_expFromRepeatStmt()
        {
            termFromRepeatStmt();
            while (CurrentToken == "+" || CurrentToken == "-" || CurrentToken == "–")
            {
                addop();
                term();
            }

        }

        public void addop(){
            if (CurrentToken == "+")
                match(CurrentToken, "+");
            else if (CurrentToken == "-")
                match(CurrentToken, "-");
            else if (CurrentToken == "–")
                match(CurrentToken, "–");
}
        public void addopFromAssignStmt()
        {
            if (CurrentToken == "+")
            {
                operator_ = new TreeNode("Operator (" + CurrentToken + ")");
                assignstmt.Nodes.Add(operator_);
                match(CurrentToken, "+");
            }
            else if (CurrentToken == "-")
            {
                operator_ = new TreeNode("Operator (" + CurrentToken + ")");
                assignstmt.Nodes.Add(operator_);
                match(CurrentToken, "-");
            }
            else if (CurrentToken == "–")
            {
                operator_ = new TreeNode("Operator (" + CurrentToken + ")");
                assignstmt.Nodes.Add(operator_);
                match(CurrentToken, "–");
            }
        }
        public void term(){
            factor();
            while(CurrentToken=="*" || CurrentToken=="/"){
                mulop();
                factor();
}
}
        public void termFromAssignStmt()
        {
            factorFromAssignStmt();
            while (CurrentToken == "*" || CurrentToken == "/")
            {
                mulopFromAssignStmt();
                factorFromAssignStmt();
            }
        }
        public void termFromIfStmt()
        {
            factorFromIfStmt();
            while (CurrentToken == "*" || CurrentToken == "/")
            {
                mulop();
                factor();
            }
        }
        public void termFromWriteStmt()
        {
            factorFromWriteStmt();
            while (CurrentToken == "*" || CurrentToken == "/")
            {
                mulop();
                factor();
            }
        }
        public void termFromRepeatStmt()
        {
            factorFromRepeatStmt();
            while (CurrentToken == "*" || CurrentToken == "/")
            {
                mulop();
                factor();
            }
        }
        public void mulop(){
            if(CurrentToken=="*")
                match(CurrentToken,"*");
            else if(CurrentToken=="/")
                match(CurrentToken,"/");
}
        public void mulopFromAssignStmt()
        {
            if (CurrentToken == "*")
            {
                operator_ = new TreeNode("Operator (" + CurrentToken + ")");
                assignstmt.Nodes.Add(operator_);
                match(CurrentToken, "*");
            }
            else if (CurrentToken == "/")
            {
                operator_ = new TreeNode("Operator (" + CurrentToken + ")");
                assignstmt.Nodes.Add(operator_);
                match(CurrentToken, "/");
            }
        }
        public void factor(){
            if(state==nameofState.NUMBER){
                ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
                TreeNode factor_2 = new TreeNode();
                
                factor_2.Text = "Number: " + CurrentToken;
                ifstmt.Nodes.Add(factor_2);

                getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
}
            else if (state==nameofState.IDENTIFIER){
                TreeNode factor_2 = new TreeNode();
                
                factor_2.Text = "Identifier " + CurrentToken;
                ifstmt.Nodes.Add(factor_2);
                match("IDENTIFIER", state.ToString());

}
            else{
                match(CurrentToken,"(");
                exp();
                match(CurrentToken,")");
}

}
        public void factorFromAssignStmt()
        {
            if (state == nameofState.NUMBER)
            {
                ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Number: " + CurrentToken;
                assignstmt.Nodes.Add(factor_2);

                getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
            }
            else if (state == nameofState.IDENTIFIER)
            {
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Identifier " + CurrentToken;
                assignstmt.Nodes.Add(factor_2);
                match("IDENTIFIER", state.ToString());

            }
            else
            {
                match(CurrentToken, "(");
                exp();
                match(CurrentToken, ")");
            }

        }
        public void factorFromIfStmt()
        {
            if (state == nameofState.NUMBER)
            {
                ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Number: " + CurrentToken;
                ifstmt.Nodes.Add(factor_2);

                getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
            }
            else if (state == nameofState.IDENTIFIER)
            {
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Identifier " + CurrentToken;
                ifstmt.Nodes.Add(factor_2);
                match("IDENTIFIER", state.ToString());

            }
            else
            {
                match(CurrentToken, "(");
                exp();
                match(CurrentToken, ")");
            }

        }
        public void factorFromWriteStmt()
        {
            if (state == nameofState.NUMBER)
            {
                ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Number: " + CurrentToken;
                writestmt.Nodes.Add(factor_2);

                getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
            }
            else if (state == nameofState.IDENTIFIER)
            {
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Identifier " + CurrentToken;
                writestmt.Nodes.Add(factor_2);
                match("IDENTIFIER", state.ToString());

            }
            else
            {
                match(CurrentToken, "(");
                exp();
                match(CurrentToken, ")");
            }

        }
        public void factorFromRepeatStmt()
        {
            if (state == nameofState.NUMBER)
            {
                ParseTextBox.Text += "" + CurrentToken + "             " + state.ToString() + "\n";
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Number: " + CurrentToken;
                repeatstmt.Nodes.Add(factor_2);

                getNexttokenParser(code2, ref CurrentToken, ref startFromHere, ref state);
            }
            else if (state == nameofState.IDENTIFIER)
            {
                TreeNode factor_2 = new TreeNode();

                factor_2.Text = "Identifier " + CurrentToken;
                repeatstmt.Nodes.Add(factor_2);
                match("IDENTIFIER", state.ToString());

            }
            else
            {
                match(CurrentToken, "(");
                exp();
                match(CurrentToken, ")");
            }

        }




        private void RestartButtonClicked(object sender, EventArgs e)
        {
            TokensLabel.Text = "";
            TokensLabel.Hide();
            LexemeTextBox.Text = "";
            state = nameofState.START;
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            //TreeNode treenode = new TreeNode("windows");
            //TreeNode nodes1 = new TreeNode("Windows 8");
            //TreeNode nodes2 = new TreeNode("Windows 7");
            //TreeNode[] arr = new TreeNode[] { nodes1, nodes2 };
            //treenode = new TreeNode("Windows", arr);
            //treeView1.Nodes.Add(treenode);
            
            ParserLaunch();
            SyntaxTree.ExpandAll();
            

        }

        private void ParseTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        
        

    }
}




