using Python.Runtime;
using System;
using System.IO;
using System.Linq;
using UnityEngine;


    public class Gesticulator : MonoBehaviour
    {
        PyGesticulatorTestor pyGesticulatorTestor;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Start");
            pyGesticulatorTestor = new PyGesticulatorTestor();
            pyGesticulatorTestor.Test();
            Debug.Log("End");
        }
    }

    class PyGesticulatorTestor
    {
        void AddEnvPath(params string[] paths)
        {      // PC에 설정되어 있는 환경 변수를 가져온다.
            var envPaths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator).ToList();
            // 중복 환경 변수가 없으면 list에 넣는다.
            envPaths.InsertRange(0, paths.Where(x => x.Length > 0 && !envPaths.Contains(x)).ToArray());
            // 환경 변수를 다시 설정한다.
            Environment.SetEnvironmentVariable("PATH", string.Join(Path.PathSeparator.ToString(), envPaths), EnvironmentVariableTarget.Process);
        }
    
        public PyGesticulatorTestor()
        {
            Runtime.PythonDLL = @"C:\Users\jongh\Anaconda3\envs\gest_env_py37\python37.dll";
            var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\jongh\Anaconda3\envs\gest_env_py37");
            AddEnvPath(PYTHON_HOME, Path.Combine(PYTHON_HOME, @"Library\bin"));
            PythonEngine.PythonHome = PYTHON_HOME;
            PythonEngine.PythonPath = string.Join
            (
                Path.PathSeparator.ToString(),
                new string[]
                {
                      PythonEngine.PythonPath,
                      Path.Combine(PYTHON_HOME, @"Lib\site-packages"),
                      @"C:\Users\jongh\OneDrive\바탕 화면\Metaver_Project_120220121_Shinjonghyun\pythonGesticulator"
                }
            );
            PythonEngine.Initialize();
        }
    
    
        void Add_PySysPath(string path)
        {
            dynamic pysys = Py.Import("sys");   // import sys module from  PythonEngine.PythonPath 
            string[] sysPathArray = (string[])pysys.path;
    
            string EnvPath = path;
            if (sysPathArray.Contains(EnvPath) == false)
                pysys.path.append(EnvPath);
    
        }
    
        public void Test()
        {
            using (Py.GIL())
            {
                dynamic os = Py.Import("os");
                dynamic pycwd = os.getcwd();
    
                string cwd = (string)pycwd;
                Debug.Log($"[before]cwd:{cwd}");
                Add_PySysPath(path: @"C:\Users\jongh\OneDrive\바탕 화면\Metaver_Project_120220121_Shinjonghyun\pythonGesticulator");
                Add_PySysPath(path: @"C:\Users\jongh\OneDrive\바탕 화면\Metaver_Project_120220121_Shinjonghyun\pythonGesticulator\gesticulator\visualization");
    
                dynamic demo_py = Py.Import("demo.demo");
                string audioPath = @"C:\Users\jongh\OneDrive\바탕 화면\Metaver_Project_120220121_Shinjonghyun\pythonGesticulator\demo\input\jeremy_howard.wav";
                string text = "Deep learning is an algorithm inspired by how the human brain works, and as a result it's an algorithm which has no theoretical limitations on what it can do. The more data you give it and the more computation time you give it, the better it gets. The New York Times also showed in this article another extraordinary result of deep learning which I'm going to show you now. It shows that computers can listen and understand.";
                demo_py.CreateBVH_InUnity(audioPath, text);
    
                os.chdir(cwd);
                Debug.Log($"[after]cwd:{cwd}");
            }
        }
    }


