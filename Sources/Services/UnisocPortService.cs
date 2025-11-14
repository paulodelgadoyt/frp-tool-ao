using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using iReverse_UniSPD_FRP.My;

namespace iReverse_UniSPD_FRP.Services
{
    /// <summary>
    /// Serviço responsável por gerenciar conexões de porta COM com dispositivos Unisoc
    /// </summary>
    public class UnisocPortService : IDisposable
    {
        private MySerialDevice _serialDevice;
        private bool _disposed = false;

        /// <summary>
        /// Porta COM atual
        /// </summary>
        public string PortCom { get; private set; }

        /// <summary>
        /// Indica se está conectado
        /// </summary>
        public bool IsConnected => _serialDevice?.m_port?.IsOpen ?? false;

        /// <summary>
        /// Timeout de leitura/escrita em ms
        /// </summary>
        public int Timeout { get; set; } = 500;

        /// <summary>
        /// Evento disparado quando a conexão é estabelecida
        /// </summary>
        public event EventHandler<string> Connected;

        /// <summary>
        /// Evento disparado quando a conexão é perdida
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Conecta à porta COM especificada
        /// </summary>
        public async Task<bool> ConnectAsync(string portCom, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(portCom))
            {
                throw new ArgumentException("Porta COM não pode ser vazia", nameof(portCom));
            }

            try
            {
                // Limpa conexão anterior se existir
                Disconnect();

                PortCom = portCom.Replace("COM", "").Trim();
                SerialPort serialPort = new SerialPort($"COM{PortCom}", 115200)
                {
                    ReadTimeout = 120000,
                    WriteTimeout = 120000
                };

                _serialDevice = new MySerialDevice(serialPort);
                MySerialDevice.maxtimeout = Timeout;
                Main.myserial = _serialDevice;

                await _serialDevice.ConnectAsync();

                if (_serialDevice.m_port.IsOpen)
                {
                    Connected?.Invoke(this, PortCom);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao conectar na porta COM{portCom}: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
                return false;
            }
        }

        /// <summary>
        /// Desconecta da porta COM
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _serialDevice?.Dispose();
                _serialDevice = null;
                PortCom = null;
                Main.myserial = null;
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MyDisplay.RichLogs($"Erro ao desconectar: {ex.Message}", 
                    System.Drawing.Color.Red, true, true);
            }
        }

        /// <summary>
        /// Obtém a instância do dispositivo serial (para uso interno)
        /// </summary>
        internal MySerialDevice GetSerialDevice()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Não há conexão ativa");
            }
            return _serialDevice;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Disconnect();
                _disposed = true;
            }
        }
    }
}

