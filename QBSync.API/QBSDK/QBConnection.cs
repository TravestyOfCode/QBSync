using QBXMLRP2Lib;

namespace QBSync.API.QBSDK
{
    public class QBConnection : IDisposable
    {
        private bool disposedValue;
        private IRequestProcessor5? _rp;
        private string? _ticket;
        private readonly ILogger<QBConnection> _logger;

        public string? QBFile { get; set; }
        public string AppID { get; set; } = "QBSDK";
        public QBFileMode FileMode { get; set; } = QBFileMode.qbFileOpenDoNotCare;

        public QBConnection(ILogger<QBConnection> logger)
        {
            _logger = logger;
        }

        public async Task<string?> ProcessRequest(string request, CancellationToken cancellationToken = default)
        {
            if (!Open())
            {
                return await Task.FromResult((string?)null);
            }

            return _rp!.ProcessRequest(_ticket, request);
        }

        private bool Open()
        {
            if (IsCorrectFileOpen())
            {
                return true;
            }

            Close();

            try
            {
                _rp = new RequestProcessor3();
                _rp.OpenConnection2(AppID, AppID, QBXMLRPConnectionType.localQBD);
                _ticket = _rp.BeginSession(QBFile, FileMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error trying to open connection with QuickBooks.");
                return false;
            }

            return true;
        }

        private bool IsCorrectFileOpen()
        {
            if (_rp == null || _ticket.IsNullOrEmpty())
            {
                return false;
            }

            try
            {
                var currentCompany = _rp.GetCurrentCompanyFileName(_ticket);

                // If the requested file is null or empty, any company connection is ok.
                if (QBFile.IsNullOrEmpty())
                {
                    return true;
                }

                return System.IO.Path.GetFullPath(currentCompany).Equals(System.IO.Path.GetFullPath(QBFile!), StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error trying to get current company file from QuickBooks.");
                return false;
            }
        }

        private void Close()
        {
            EndSession();
            CloseConnection();
        }

        private void EndSession()
        {
            try
            {
                if (_rp != null && _ticket.IsNotNullOrEmpty())
                {
                    _rp.EndSession(_ticket);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error Ending Session with QuickBooks.");
            }
            finally
            {
                _ticket = null;
            }
        }

        private void CloseConnection()
        {
            try
            {
                if (_rp != null)
                {
                    _rp.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error closing connection with QuickBooks.");
            }
            finally
            {
                _rp = null;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                Close();
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // Override finalizer as 'Dispose(bool disposing)' has code to free unmanaged resources
        ~QBConnection()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
