﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Commands
{
    public class ExecJavascript :Command
    {
        public ExecJavascript(String id, String description, KeyValuePair<String, Tuple<Type, String>> output, Dictionary<String, Tuple<Type, String>> parameters)
            : base(id, description, output, parameters)
        {

        }

        public override void Run(TestContainer container)
        {
            try
            {
                //* add for IsExecuteCommand start
                if (!base.IsCommandContinue(container))
                {
                    this.PassTest = true;
                    this.SkipTest = true;
                    return;
                }
                //* add for IsExecuteCommand end
               
                OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)container.Driver;
                string strOutput = js.ExecuteScript(base.GetParameter("Statement")).ToString();

                this.PassTest = true;
                
                //* add for output and IsExpectedFail start
                this.Output = base.GetOutPut(this.Output.Key, this.Output, this.PassTest, true, strOutput);
                //* add for output and IsExpectedFail end

                //* add for output and IsExpectedFail start
                this.PassTest = GetTestPassExpected(this.PassTest);
                //* add for output and IsExpectedFail end

                //*add for ScreenShot start
                if (!this.PassTest)
                {
                    base.CommandFailScreenShot(container);
                }
                //*add for ScreenShot end

            }
            catch (Exception ex)
            {
                //*add for ScreenShot start
                base.CommandFailScreenShot(container);
                //*add for ScreenShot end
                throw ex;
            }
        }
    }
}
