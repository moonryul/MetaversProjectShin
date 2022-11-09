using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


    public class PythonNetTest : MonoBehaviour
    {
        PythonTestor pythonTestor;
        // Start is called before the first frame update
        void Start()
        {
            pythonTestor = new PythonTestor();
            pythonTestor.Test();
        }
    }



   class PythonTestor
   {
       public PythonTestor()
       {
           Runtime.PythonDLL = @"C:\Users\jongh\Anaconda3\envs\MetaVersePythonEnv\python38.dll";
           var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\jongh\Anaconda3\envs\MetaVersePythonEnv");
           PythonEngine.PythonHome = PYTHON_HOME;
           PythonEngine.PythonPath = string.Join
           (
               Path.PathSeparator.ToString(),
               new string[]
               {
                     PythonEngine.PythonPath,
                     Path.Combine(PYTHON_HOME, @"Lib\site-packages"),
                     Path.Combine(PYTHON_HOME, @"SJHLib"),
                     @"C:\Users\jongh\Anaconda3\envs\MetaVersePythonEnv\SJHLib"
               }
           );
           PythonEngine.Initialize();
       }
   
       public void Test()
       {
           using (Py.GIL())
           {
               dynamic os = Py.Import("os");
               dynamic pycwd = os.getcwd();
               string cwd = (string)pycwd;
               Debug.Log($"cwd:{cwd}");
               dynamic test = Py.Import("test");
               dynamic f = test.Calculator(1, 2);
               Console.WriteLine(f.Add());
               Console.WriteLine(test.A(1, 2));
           }
       }
   }


