using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        
        static void ToFile(string path,List<string> args)
        {
            //写入文件
            FileInfo file = new FileInfo(path);
            StreamWriter sw = file.AppendText();
            foreach(string _ in args)
            {
                sw.Write(_);
            }
            sw.Close();
        }
        static void Main(string[] args)
        {
            List<string> questions = new List<string>();
            Console.WriteLine("请输入生成题目数目：");
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                ZXSB a = new ZXSB();
                a.Comprehensive();
                Copyvalues(a.zxsb, questions);
                a = null;
            }
            //txt文件路径
            string path = @"D:\ConsoleApp1\ConsoleApp1\r.txt";
            ToFile(path, questions);
        }
        static void Copyvalues(List<string> a, List<string> b)
        {
            foreach (string _ in a)
            {
                b.Add(_);
            }
        }
    }
}
