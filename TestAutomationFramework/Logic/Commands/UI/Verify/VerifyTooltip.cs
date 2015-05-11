﻿using Logic.Commands.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Commands
{
    public class VerifyTooltip:Command
    {
        public VerifyTooltip(String id, String description, KeyValuePair<String, Tuple<Type, String>> output, Dictionary<String, Tuple<Type, String>> parameters)
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

                string invokeMethodName;
                string invokeParameter;

                UICommandHelper cmdHelp = new UICommandHelper();
                cmdHelp.ExtractMethodName(base.GetParameter("Target"), out invokeMethodName, out invokeParameter);

                By by = typeof(By).GetMethod(invokeMethodName).Invoke(null, new[] { invokeParameter }) as By;

                WebDriverWait wait = new WebDriverWait(container.Driver, TimeSpan.FromSeconds(container._commandTimeout));
                var element = wait.Until(ExpectedConditions.ElementIsVisible(by));
                
                //find tooltip 
                if (base.GetParameter("Value").Equals(container.Driver.FindElement(by).GetAttribute("data-original-title").Trim()))
                {                    
                    WebDriverWait wait2 = new WebDriverWait(container.Driver, TimeSpan.FromSeconds(container._commandTimeout));
                    var element2 = wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'tooltip')]")));
                    
                    if (IsElementPresent(By.XPath("//div[contains(@class,'tooltip')]"), container))
                    {
                        /// TODO: Pass
                        this.PassTest = true;
                    }
                }

                //* add for output and IsExpectedFail start
                this.Output = base.GetOutPut(this.Output.Key, this.Output, this.PassTest);
                //* add for output and IsExpectedFail end

                //* add for output and IsExpectedFail start
                this.PassTest = GetTestPassExpected(this.PassTest);
                //* add for output and IsExpectedFail end


                //add by zhuqianqian ScreenShot start
                if (!this.PassTest)
                {
                    CommandFailScreenShot(container);
                    Logging.SaveLog("CommandId:" + this.Id + "=>Expecte value:" + base.GetParameter("Value") + "   Actual value:" + container.Driver.FindElement(by).GetAttribute("data-original-title").Trim(), ELogType.Info);
                }
                //add by zhuqianqian ScreenShot end
            }
            catch (Exception ex)
            {
                //add by zhuqianqian ScreenShot start
                CommandFailScreenShot(container);
                throw ex;
                //add by zhuqianqian ScreenShot end
            }

        }
        private bool IsElementPresent(By by, TestContainer container)
        {
            try
            {
                container.Driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}