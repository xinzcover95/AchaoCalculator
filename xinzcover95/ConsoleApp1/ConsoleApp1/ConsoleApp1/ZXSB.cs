using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ConsoleApp1
{
    class ZXSB
    {
        //定义字符串常量
        private string Const = "+-*/";
        //定义列表保存生成的算式
        public List<string> zxsb = new List<string>();
        //定义随机种子
        private static Random rd = new Random(3);
        //清除变量内容，以便重新生成
        private void ClearVar()
        {
            this.zxsb.Clear();
        }
        //取得操作符个数,2个或者3个
        private int GetAccount()
        {
            int zxsb = rd.Next(2, 4);
            return zxsb;
        }
        //进行操作符赋值
        private void GetStr()
        {
            int index;
            for (int i = 0; i < this.GetAccount(); i++)
            {
                //随机0-3的数，根据位置映射操作符
                index = rd.Next(0, 4);
                //为了简化赋值判断
                if (!this.zxsb.Contains(Convert.ToString(Const[index])))
                {
                    //如果算式不包含此操作符则判断是否减法与除法同时出现
                    if((this.zxsb.Contains("/") && index==1) || (this.zxsb.Contains("-") && index == 3))
                    {
                        //如果减法，除法同时出现，则重新生成
                        i -= 1;
                    }
                    else
                    {
                        //否则进行添加
                        this.zxsb.Add(Convert.ToString(Const[index]));
                    }  
                }
                else
                {
                    //如果算式包含操作符，返回循环
                    i -= 1;
                }
            }
        }
        //对数进行赋值
        private void GetNums()
        {
            if(this.zxsb.Count==2)
            {
                //两个操作符时的赋值
                int num1 = rd.Next(1, 101);
                int num2 = rd.Next(1, 101);
                int num3 = rd.Next(1, 101);
                this.zxsb.Insert(0, Convert.ToString(num1));
                this.zxsb.Insert(2, Convert.ToString(num2));
                this.zxsb.Insert(4, Convert.ToString(num3));
            }
            else
            {
                //三个操作符时的赋值
                int num1 = rd.Next(1, 101);
                int num2 = rd.Next(1, 101);
                int num3 = rd.Next(1, 101);
                int num4 = rd.Next(1, 101);
                this.zxsb.Insert(0, Convert.ToString(num1));
                this.zxsb.Insert(2, Convert.ToString(num2));
                this.zxsb.Insert(4, Convert.ToString(num3));
                this.zxsb.Insert(6, Convert.ToString(num4));
            }

        }
        //判断有除法时是否为整除
        private bool JudgeDiv(int num1,int num2)
        {
            if(num1%num2==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //判断减法是否符合规则
        private bool JudgeStra(int num1, int num2)
        {
            if(num1>num2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //计算算式结果
        private string Getanswer(List<string> temp)
        {
            //将List类型转换为String
            String temp_ = "";
            for(int i=0;i<temp.Count;i++)
            {
                temp_ += temp[i];
            }
            //利用Datatable.Compute()计算结果(实质为调用SQL语句计算)
            DataTable cal = new DataTable();
            string answer = Convert.ToString(cal.Compute(Convert.ToString(temp_), null));
            return answer;
        }
        //没有除法时，先递归计算乘法在进行判定减法
        private List<string> CalMul(List<string> args)
        {
            //找出*的位置
            int index = args.IndexOf("*");
            if(index!=-1)
            {
                //取出做乘法的两个数
                int num1 = Convert.ToInt32(args[index - 1]);
                int num2 = Convert.ToInt32(args[index + 1]);
                //在列表删除num1*num2
                args.RemoveAt(index - 1);
                args.RemoveAt(index - 1);
                args.RemoveAt(index - 1);
                args.Insert(index - 1, Convert.ToString(num1 * num2));
                return this.CalMul(args);
            }
            else
            {
                return args;
            }
        }
        //综合函数，进行判断，获取答案
        public void Comprehensive()
        {
            //首先随机出符号和算式
            this.GetStr();
            this.GetNums();
            //定义一个新列表转存算式方便判断
            List<string> question = new List<string>();
            foreach(string _ in this.zxsb)
            {
                question.Add(_);
            }
            if(this.zxsb.Contains("/"))
            {
                //如果算式有除法，进行判断
                //找到/下标
                int index = this.zxsb.IndexOf("/");
                //取出做除法的两个数，进行判断
                int num1 = Convert.ToInt32(this.zxsb[index - 1]);
                int num2 = Convert.ToInt32(this.zxsb[index + 1]);
                if(this.JudgeDiv(num1,num2))
                {
                    //除法满足条件，直接计算结果存入算式中
                    string answer = this.Getanswer(this.zxsb);
                    this.zxsb.Add("=");
                    this.zxsb.Add(answer);
                    this.zxsb.Add("\n");
                }
                else
                {
                    //如果除法不满足整除，清除变量重新生成
                    this.ClearVar();
                    this.Comprehensive();
                }
            }
            else if(this.zxsb.Contains("-"))
            {
                //如果包含减法，先看是否有乘法，如果有进行计算
                if(question.Contains("*"))
                {
                    //计算乘法后存入临时列表再判断减法
                    question = this.CalMul(question);
                    int index = question.IndexOf("-");
                    int num1 = Convert.ToInt32(question[index - 1]);
                    int num2 = Convert.ToInt32(question[index + 1]);
                    if (this.JudgeStra(num1,num2))
                    {
                        //如果减法满足条件计算结果，保存算式
                        string answer = this.Getanswer(this.zxsb);
                        this.zxsb.Add("=");
                        this.zxsb.Add(answer);
                        this.zxsb.Add("\n");;
                    }
                    else
                    {
                        //如果减法不满足条件，清除变量重新生成
                        this.ClearVar();
                        this.Comprehensive();
                    }
                }
            }
            else
            {
                //如果没有除法减法，直接计算结果保存算是
                string answer = this.Getanswer(this.zxsb);
                this.zxsb.Add("=");
                this.zxsb.Add(answer);
                this.zxsb.Add("\n");
                Console.WriteLine();
            }
        }
    }
}
