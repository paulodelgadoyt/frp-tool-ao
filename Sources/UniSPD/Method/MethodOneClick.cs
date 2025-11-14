using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;
using iReverse_UniSPD_FRP.ViewModels;

namespace iReverse_UniSPD_FRP.UniSPD.Method
{
    internal static class MethodOneClick
    {
        public static void SPDOneClickExecModel()
        {
            try
            {
                if (!String.IsNullOrEmpty(MyListSPDDevice.Brand))
                {
                    int num = 0;
                    Main.SharedUI.PanelSPDOneClick.Controls.Clear();

                    string str = File.ReadAllText(
                        Application.StartupPath
                            + "\\Data\\Models\\"
                            + MyListSPDDevice.Brand.ToUpper()
                            + "\\"
                            + MyListSPDDevice.ModelName.ToUpper()
                            + ".txt"
                    );

                    using (var stringReader = new StringReader(str))
                    {
                        while (stringReader.Peek() != -1)
                        {
                            string text = stringReader.ReadLine();
                            if (text.Contains("FDL1Address"))
                            {
                                MethodDownload.fdl1_addr = Convert.ToInt32(
                                    text.Replace(" ", "")
                                        .Replace("FDL1Address:", "")
                                        .Replace("0x", ""),
                                    16
                                );
                                Console.WriteLine(
                                    "FDL1 Address : "
                                        + text.Replace(" ", "").Replace("FDL1Address:", "")
                                );
                            }
                            else if (text.Contains("FDL2Address"))
                            {
                                MethodDownload.fdl2_addr = Convert.ToInt32(
                                    text.Replace(" ", "")
                                        .Replace("FDL2Address:", "")
                                        .Replace("0x", ""),
                                    16
                                );
                                Console.WriteLine(
                                    "FDL2 Address : "
                                        + text.Replace(" ", "").Replace("FDL2Address:", "")
                                );
                            }
                            else
                            {
                                Button BtnSPDOneClick = new Button
                                {
                                    Anchor =
                                        AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                                };
                                BtnSPDOneClick.ForeColor = Color.FromArgb(64, 64, 64);

                                BtnSPDOneClick.Location = new Point(2, num);
                                BtnSPDOneClick.Size = new Size(212, 23);
                                BtnSPDOneClick.TabIndex = 36;
                                BtnSPDOneClick.Text = text;
                                BtnSPDOneClick.TextAlign = ContentAlignment.MiddleLeft;
                                Main.SharedUI.PanelSPDOneClick.Controls.Add(BtnSPDOneClick);
                                num += 27;

                                BtnSPDOneClick.Click += SPDDoExecOneClick;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), null, MessageBoxButtons.OK);
            }
        }

        public static async void SPDDoExecOneClick(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string operation = button.Text;

                // Obtém ViewModel do Main
                MainViewModel viewModel = Main.SharedUI?.ViewModel;
                if (viewModel == null)
                {
                    MyDisplay.RichLogs("Erro: ViewModel não disponível", Color.Red, true, true);
                    return;
                }

                MyProgress.Watch.Restart();
                MyProgress.Watch.Start();
                
                MyDisplay.RtbClear();
                MyDisplay.RichLogs("Operation	     : ", Color.Black, true, false);
                MyDisplay.RichLogs(operation, Color.Orange, true, true);
                MyDisplay.RichLogs(" Brand		     : ", Color.Black, true, false);
                MyDisplay.RichLogs(MyListSPDDevice.Brand, Color.Purple, true, true);
                
                string MyDevices = MyListSPDDevice.DevicesName.Replace(MyListSPDDevice.Brand, "");
                if (!String.IsNullOrEmpty(MyDevices))
                {
                    MyDisplay.RichLogs(" Devices	     :", Color.Black, true, false);
                    MyDisplay.RichLogs(MyDevices, Color.Purple, true, true);
                }
                
                MyDisplay.RichLogs(" Model		     : ", Color.Black, true, false);
                MyDisplay.RichLogs(MyListSPDDevice.ModelName, Color.Purple, true, true);
                MyDisplay.RichLogs(" Platform	     : ", Color.Black, true, false);
                MyDisplay.RichLogs("Spreadtrum", Color.Purple, true, true);
                MyDisplay.RichLogs(" Connect	     : ", Color.Black, true, false);
                MyDisplay.RichLogs("Download", Color.Purple, true, true);
                MyDisplay.RichLogs("Loader Data	     : ", Color.Black, true, false);

                // Carrega e faz download dos arquivos FDL via ViewModel
                bool fdlLoaded = await viewModel.LoadAndDownloadFDLAsync(
                    MyListSPDDevice.Brand, 
                    MyListSPDDevice.ModelName
                );

                if (!fdlLoaded)
                {
                    MyDisplay.RichLogs("Falha ao carregar FDL. Operação cancelada.", 
                        Color.Red, true, true);
                    return;
                }

                MyDisplay.RichLogs("Done  ✓ ", Color.Purple, true, true);
                MyDisplay.RichLogs("Support Data	     : ", Color.Black, true, false);
                MyDisplay.RichLogs("Done  ✓ ", Color.Purple, true, true);
                MyDisplay.RichLogs(" ", Color.Purple, true, true);
                Thread.Sleep(200);

                // Executa operação FRP via ViewModel
                await viewModel.ExecuteFRPOperationAsync(operation);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Your task was canceled.");
                Main.SharedUI?.CkFDLLoaded?.Invoke(
                    new Action(() => Main.SharedUI.CkFDLLoaded.Checked = false)
                );
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro na execução: {ex.Message}", Color.Red, true, true);
                Console.WriteLine($"Erro: {ex}");
            }
        }

        public static byte[] GetSPDFile(string namafile, bool pbar)
        {
            byte[] result = null;
            try
            {
                result = File.ReadAllBytes(
                    Application.StartupPath
                        + "\\Data\\Models\\"
                        + MyListSPDDevice.Brand.ToUpper()
                        + "\\"
                        + MyListSPDDevice.ModelName.ToUpper()
                        + "\\"
                        + namafile
                );
            }
            catch (Exception)
            {
                result = new byte[0];
            }
            return result;
        }
    }
}
