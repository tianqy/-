using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * 递归下降分析程序
 * 程序思路：记录输入的表达式，依照顺序每次取表达式的第一个字符与文法产生式比较，
 * 若匹配上则删去首字符，继续分析，若发现不匹配则出错并结束分析
 * 注： 输入表达式默认以 # 结束，程序允许输入时不带 # ，但是如果附带 # 后，
 * 默认表达式在此处结束，后面的内容对于程序分析不会造成影响
 */
namespace recursion_down_analysis
{
    public partial class Form1 : Form
    {
        char[] follow_A1 = { ')', 'b' };    // A'的FOLLOW集合
        string expression = "";     // 表达式

        Label lblTitle = new Label();   // 标题标签
        Label lbl_grammar = new Label();    // 文法标签
        RichTextBox rtb_grammar = new RichTextBox();    // 文法区
        Label lbl_expression = new Label(); // 表达式标签
        TextBox tb_expression = new TextBox();  // 表达式编辑区
        Button btn_analysis = new Button();  // 分析按钮
        Label lbl_result = new Label();    // 分析结果标签

        // 窗体初始化
        public Form1()
        {
            InitializeComponent();
            initForm(); //初始化界面控件
            initData(); //初始化界面数据
            btn_analysis.Click += btn_analysis_Click;   // 添加事件监听
        }

        // 初始化界面数据
        private void initData()
        {
            lblTitle.Text = "递归下降程序分析";
            lbl_grammar.Text = "文法：";
            lbl_expression.Text = "表达式：";
            btn_analysis.Text = "分析";
            string grammar_text = "S->(A)|aAb\r\nA->eA'|dSA'\r\nA'->dA'|ε";
            rtb_grammar.Text = grammar_text;
        }

        // 初始化界面布局
        private void initForm()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;    // 设置窗口不可拉伸
            this.MaximizeBox = false;   // 设置窗口禁用最大化
            this.Size = new Size(600 * 500 / 660, 275 * 500 / 660); //设置窗口大小为 600 * 275， 后面的比例如果没有的话在本人电脑上窗体大小会被放大
            this.Font = new System.Drawing.Font("宋体", 12);  // 设置默认字体

            lblTitle.Size = new Size(171, 19);  // 设置控件大小
            lblTitle.Location = new Point(194, 9);  // 设置控件位置
            lblTitle.Font = new Font("宋体", 15); // 设置字体
            this.Controls.Add(lblTitle);    // 将控件加入界面控件集合中，显示控件
            // 以下设置属性同上介绍
            lbl_grammar.Size = new Size(60, 16);
            lbl_grammar.Location = new Point(9, 56);
            this.Controls.Add(lbl_grammar);

            rtb_grammar.Size = new Size(191, 126);
            rtb_grammar.Location = new Point(12, 75);
            rtb_grammar.Font = new Font("MV Boli", 12);
            this.Controls.Add(rtb_grammar);

            lbl_expression.Size = new Size(72, 16);
            lbl_expression.Location = new Point(319, 56);
            this.Controls.Add(lbl_expression);

            tb_expression.Size = new Size(243, 26);
            tb_expression.Location = new Point(322, 76);
            this.Controls.Add(tb_expression);

            btn_analysis.Size = new Size(75, 32);
            btn_analysis.Location = new Point(322, 120);
            this.Controls.Add(btn_analysis);

            lbl_result.Size = new Size(180, 16);
            lbl_result.Location = new Point(320, 170);
            this.Controls.Add(lbl_result);
        }

        // 解析S->(A)|aAb
        void S()    
        {
            if (expression[0] == '(')   // 判断S->（A), 先判断（
            {
                expression = expression.Substring(1);   // 删除表达式当前首元素
                A();    // 判断产生式A
                if (expression[0] == ')')   // 判断）
                {
                    expression = expression.Substring(1);   // 删除表达式当前首元素
                    return; // 完成S->(A)的查找
                }
                else
                {
                    lbl_result.Text = "error!"; // 错误标记
                    return;
                }
            }
            else if (expression[0] == 'a')  // 判断S->aAb, 先判断a
            {
                expression = expression.Substring(1);
                A();    // 判断产生式A
                if (expression[0] == 'b')   // 判断b
                {
                    expression = expression.Substring(1);
                    return;
                }
                else
                {
                    lbl_result.Text = "error!"; // 错误标记
                }
            }
            else
            {
                lbl_result.Text = "error!"; // 错误标记
                return;
            }
        }

        // 解析A->eA'|dSA'
        void A()
        {
            if (expression[0] == 'e')   // 判断A->eA'
            {
                expression = expression.Substring(1);
                A1();
            }
            else if (expression[0] == 'd')  // 判断A->dSA'
            {
                expression = expression.Substring(1);
                S();
                A1();
            }
            else
            {
                lbl_result.Text = "error!"; //  错误标记
                return;
            }
        }

        // 解析A'->dA'|ε
        void A1()
        {
            if (expression[0] == 'd')   // 判断A'->dA'
            {
                expression = expression.Substring(1);
                A1();
            }
            else if (follow_A1.Contains(expression[0])) // 判断A'->ε, 表达式为空时判断follow集合
            {
                return;
            }
            else
            {
                lbl_result.Text = "error!"; // 错误标记
                return;
            }
        }

        //开始分析事件
        private void btn_analysis_Click(object sender, EventArgs e)
        {
            expression = tb_expression.Text.ToString() + "#";   // 获取表达式
            S();    // 表达式分析
            // 根据分析结果显示结果信息，若判断为否定则以红色显示，否则以绿色显示
            if (lbl_result.Text.ToString().Equals("error!"))
            {
                lbl_result.Text = "不是文法正确的句子";
                lbl_result.ForeColor = Color.FromArgb(0xdd, 0, 0);
            }
            else if(expression[0] == '#')
            {
                lbl_result.Text = "是文法正确的句子";
                lbl_result.ForeColor = Color.FromArgb(0, 0xdd, 0);
            }
            else
            {
                lbl_result.Text = "不是文法正确的句子";
                lbl_result.ForeColor = Color.FromArgb(0xdd, 0, 0);
            }
        }

    }
}
