using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.My;
using iReverse_UniSPD_FRP.UniSPD;
using iReverse_UniSPD_FRP.UniSPD.Method;

namespace iReverse_UniSPD_FRP.Services
{
    /// <summary>
    /// Serviço responsável por operações de flash/download (FDL1, FDL2)
    /// </summary>
    public class FlashService
    {
        private readonly UnisocPortService _portService;

        public FlashService(UnisocPortService portService)
        {
            _portService = portService ?? throw new ArgumentNullException(nameof(portService));
        }

        /// <summary>
        /// Carrega arquivos FDL do modelo especificado
        /// </summary>
        public FDLFiles LoadFDLFiles(string brand, string modelName)
        {
            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(modelName))
            {
                throw new ArgumentException("Brand e ModelName são obrigatórios");
            }

            string fdl1Path = Path.Combine(
                Application.StartupPath,
                "Data",
                "Models",
                brand.ToUpper(),
                modelName.ToUpper(),
                "fdl1-sign.bin"
            );

            string fdl2Path = Path.Combine(
                Application.StartupPath,
                "Data",
                "Models",
                brand.ToUpper(),
                modelName.ToUpper(),
                "fdl2-sign.bin"
            );

            byte[] fdl1 = File.Exists(fdl1Path) ? File.ReadAllBytes(fdl1Path) : new byte[0];
            byte[] fdl2 = File.Exists(fdl2Path) ? File.ReadAllBytes(fdl2Path) : new byte[0];

            return new FDLFiles
            {
                FDL1 = fdl1,
                FDL2 = fdl2,
                FDL1Address = 0,
                FDL2Address = 0
            };
        }

        /// <summary>
        /// Carrega endereços FDL do arquivo de configuração do modelo
        /// </summary>
        public void LoadFDLAddresses(string brand, string modelName, FDLFiles fdlFiles)
        {
            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(modelName))
            {
                return;
            }

            try
            {
                string configPath = Path.Combine(
                    Application.StartupPath,
                    "Data",
                    "Models",
                    brand.ToUpper(),
                    modelName.ToUpper() + ".txt"
                );

                if (!File.Exists(configPath))
                {
                    return;
                }

                string[] lines = File.ReadAllLines(configPath);
                foreach (string line in lines)
                {
                    if (line.Contains("FDL1Address"))
                    {
                        string addrStr = line.Replace(" ", "")
                            .Replace("FDL1Address:", "")
                            .Replace("0x", "");
                        if (int.TryParse(addrStr, System.Globalization.NumberStyles.HexNumber, null, out int addr))
                        {
                            fdlFiles.FDL1Address = addr;
                        }
                    }
                    else if (line.Contains("FDL2Address"))
                    {
                        string addrStr = line.Replace(" ", "")
                            .Replace("FDL2Address:", "")
                            .Replace("0x", "");
                        if (int.TryParse(addrStr, System.Globalization.NumberStyles.HexNumber, null, out int addr))
                        {
                            fdlFiles.FDL2Address = addr;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao carregar endereços FDL: {ex.Message}", 
                    System.Drawing.Color.Orange, true, true);
            }
        }

        /// <summary>
        /// Executa o download de FDL1 e FDL2
        /// </summary>
        public async Task<bool> DownloadFDLAsync(FDLFiles fdlFiles, CancellationToken cancellationToken = default)
        {
            if (!_portService.IsConnected)
            {
                throw new InvalidOperationException("Porta COM não está conectada");
            }

            if (fdlFiles == null)
            {
                throw new ArgumentNullException(nameof(fdlFiles));
            }

            // Configura os arquivos FDL no MethodDownload
            MethodDownload.fdl1 = fdlFiles.FDL1;
            MethodDownload.fdl1_len = fdlFiles.FDL1.Length;
            MethodDownload.fdl1_addr = fdlFiles.FDL1Address;

            MethodDownload.fdl2 = fdlFiles.FDL2;
            MethodDownload.fdl2_len = fdlFiles.FDL2.Length;
            MethodDownload.fdl2_addr = fdlFiles.FDL2Address;

            // Executa o download
            try
            {
                await MethodDownload.ConnectDownload(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao executar download FDL: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
        }

        /// <summary>
        /// Verifica se os arquivos FDL estão carregados
        /// </summary>
        public bool IsFDLLoaded()
        {
            return Main.SharedUI?.CkFDLLoaded?.Checked ?? false;
        }
    }

    /// <summary>
    /// Classe para armazenar dados dos arquivos FDL
    /// </summary>
    public class FDLFiles
    {
        public byte[] FDL1 { get; set; }
        public byte[] FDL2 { get; set; }
        public int FDL1Address { get; set; }
        public int FDL2Address { get; set; }
    }
}

