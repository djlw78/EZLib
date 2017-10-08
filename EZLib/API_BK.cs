﻿using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace EZLib
{
    internal class APIAccessBK
    {
        internal static string currentCaptcha;
        internal static string currentUsername;
        internal static string currentProgramId;
        internal static string currentProgramName;

        internal static string baseUrl = "http://localhost/Web_Server/api/endpoint.php?"; // API Base
        private static string authCode = "mtgEcuTSDUOW3vDDEbY6"; // Keep this a secret

        internal static Form mainForm = new mainControl();
        internal static Form loaderForm = new UserControls.Loader_Form.formLoader();
        internal static Form licenseForm = new UserControls.License_Form.formLicense();

        public static void registerApi(string username, string password)
        {
            try
            {
                string inputUsername = username;
                string inputPassword = password;
                string inputIPAddress = ipAddressApi();
                string inputHardwareId = Hardware_ID.Generate();

                string webResponse;
                string postData = "action=register&authCode=" + authCode + "&username=" + inputUsername + "&password=" + inputPassword + "&ip_address=" + inputIPAddress + "&hardware_id=" + inputHardwareId;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);

                    if (webResponse.Contains("success"))
                    {
                        messageHandler("You have successfully registered", "success");
                    } else if (webResponse.Contains("error"))
                    {
                        if (webResponse.Contains("Parameter missing"))
                        {
                            Environment.Exit(0);
                        } else if (webResponse.Contains("User already exists"))
                        {
                            messageHandler("This username is already taken", "warning");
                        } else if (webResponse.Contains("IP address found"))
                        {
                            messageHandler("You have already regisrted once", "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
            }
        }
        public static void loginApi(string username, string password)
        {
            try
            {
                string inputUsername = username;
                string inputPassword = password;
                string inputHardwareId = Hardware_ID.Generate();

                string webResponse;
                string postData = "action=authenticate&authCode=" + authCode + "&username=" + inputUsername + "&password=" + inputPassword + "&hardware_id=" + inputHardwareId;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);

                    if (webResponse.Contains("success"))
                    {
                        currentUsername = inputUsername;
                        mainForm.Visible = false;

                        string postData2 = "action=isLicensed&authCode=" + authCode + "&username=" + inputUsername + "&programId=" + currentProgramId;
                        string webResponse2;

                        using (WebClient webClient2 = new WebClient())
                        {
                            webClient2.Proxy = null;
                            webClient2.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                            webResponse2 = webClient2.DownloadString(baseUrl + postData2);

                            if (webResponse2.Contains("success"))
                            {
                                UserControls.loaderControl loaderControl = new UserControls.loaderControl();

                                loaderForm.Controls.Add(loaderControl);

                                loaderForm.StartPosition = FormStartPosition.CenterScreen;
                                loaderForm.ShowIcon = false;
                                loaderForm.ShowInTaskbar = false;

                                loaderForm.ShowDialog();
                            } else if (webResponse2.Contains("error"))
                            {
                                licenseForm.StartPosition = FormStartPosition.CenterScreen;
                                licenseForm.ShowIcon = false;
                                licenseForm.ShowInTaskbar = false;

                                licenseForm.ShowDialog();
                            }
                        }
                    }
                    else if (webResponse.Contains("error"))
                    {
                        if (webResponse.Contains("Parameter missing"))
                        {
                            Environment.Exit(0);
                        }
                        else if (webResponse.Contains("Password is incorrect"))
                        {
                            messageHandler("Your password is incorrect, please try again", "warning");
                        }
                        else if (webResponse.Contains("Hardware ID does not match"))
                        {
                            messageHandler("Your Hardware ID does not match", "error");
                        } else if (webResponse.Contains("User does not exist"))
                        {
                            messageHandler("This account does not exist", "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
            }
        }
        public static void authProgramApi(string programId)
        {
            try
            {
                string inputId = programId;

                string webResponse;
                string postData = "action=authenticateProgram&authCode=" + authCode + "&programId=" + inputId;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);

                    if (webResponse.Contains("error"))
                    {
                        messageHandler("This program ID is either banned or does not exist", "error");
                        Environment.Exit(0);
                    }
                    else
                    {
                        currentProgramId = inputId;
                        currentProgramName = webResponse;

                        mainForm.ShowIcon = false;
                        mainForm.ShowInTaskbar = false;
                        mainForm.StartPosition = FormStartPosition.CenterScreen;

                        mainForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
            }
        }
        public static void ezlibException(string programId, Exception ex)
        {
            try
            {
                string inputId = programId;
                string inputName = ex.GetType().Name;
                string inputMessage = ex.Message;

                string webResponse;
                string postData = "action=logException&authCode=" + authCode + "&programId=ezlib&exceptionName=" + inputName + "&exceptionMessage=" + inputMessage;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);
                }
            }
            catch (Exception exception)
            {
                ezlibExceptionHandler(exception);
            }
        }
        public static void licenseUserApi(string programId, string username, string licenseKey)
        {
            try
            {
                string inputId = programId;
                string inputUsername = username;
                string inputLicense = licenseKey;

                string webResponse;
                string postData = "action=licenseUser&authCode=" + authCode + "&username=" + inputUsername + "&programId=" + inputId + "&licenseKey=" + inputLicense;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);

                    if (webResponse.Contains("success"))
                    {
                        licenseForm.Close();

                        loaderForm.StartPosition = FormStartPosition.CenterParent;
                        loaderForm.ShowIcon = false;
                        loaderForm.ShowInTaskbar = false;

                        UserControls.loaderControl loaderControl = new UserControls.loaderControl();

                        loaderForm.Controls.Add(loaderControl);

                        loaderForm.ShowDialog();
                    } else
                    {
                        messageHandler("This license key does not exist or already in use", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
            }
        }

        public static string ipAddressApi()
        {
            try
            {
                string webResponse;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString("http://icanhazip.com/");
                    return webResponse;
                }
            }
            catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
                return "An error has occurred";
            }
        }
        public static string randomCaptchaApi()
        {
            try
            {
                string webResponse;
                string postData = "action=randomCaptcha&authCode=" + authCode;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);

                    currentCaptcha = webResponse;
                    return currentCaptcha;
                }
            } catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
                return "An error has occurred";
            }
        }
        public static string licenseInformation(string programId, string username, string information)
        {
            try
            {
                string inputId = programId;
                string inputUsername = username;
                string inputInformation = information;

                string webResponse;
                string postData = "action=licenseInformation&authCode=" + authCode + "&programId=" + inputId + "&username=" + inputUsername + "&information=" + inputInformation;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "EZLib 1.0 +https://ezlib.rocks/");
                    webResponse = webClient.DownloadString(baseUrl + postData);

                    return webResponse;
                }
            }
            catch (Exception ex)
            {
                ezlibExceptionHandler(ex);
                return "An error has occurred";
            }
        }

        internal static void ezlibExceptionHandler(Exception ex)
        {
            ezlibException(currentProgramId, ex);

            Form formControl = new UserControls.Error_Messages.formMessage(ex.GetType().Name, ex.Message);

            formControl.StartPosition = FormStartPosition.CenterScreen;
            formControl.ShowIcon = false;
            formControl.ShowInTaskbar = false;

            formControl.ShowDialog();
        } // Only for EZLib exceptions
        internal static void messageHandler(string message, string type)
        {
            Form formControl = new UserControls.Alert_Messages.formMessage();

            formControl.StartPosition = FormStartPosition.Manual;
            formControl.ShowIcon = false;
            formControl.ShowInTaskbar = false;
            formControl.Opacity = 95;
            formControl.Top = 60;
            formControl.Left = Screen.PrimaryScreen.Bounds.Width - formControl.Width - 60;

            UserControls.Alert_Messages.alertMessage alertMessage = new UserControls.Alert_Messages.alertMessage();
            alertMessage.message(message, type);

            formControl.Controls.Add(alertMessage);

            formControl.ShowDialog();
        }
    }
}