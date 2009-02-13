using System.IO;
using ZetaHtmlTidy;
using System.Text;
using System.Text.RegularExpressions;

namespace PaulStovell.Web.Application.Modules
{
    public class HtmlTidyStream : Stream
    {
        private static readonly Regex _generatorRemoval = new Regex(@"^.*?meta.*?generator.*?HTML.*?\n", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly Stream _httpResponseStream;
        private readonly StringWriter _bufferWriter;
        private readonly MemoryStream _bufferStream;
        
        public HtmlTidyStream(Stream stream)
        {
            _httpResponseStream = stream;
            _bufferWriter = new StringWriter();
            _bufferStream = new MemoryStream();
        }

        public override bool CanRead
        {
            get { return _httpResponseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _httpResponseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _httpResponseStream.CanWrite; }
        }

        public override void Flush()
        {
            _httpResponseStream.Flush();
        }

        public override long Length
        {
            get { return _httpResponseStream.Length; }
        }

        public override long Position
        {
            get { return _httpResponseStream.Position; }
            set { _httpResponseStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _httpResponseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _httpResponseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _bufferWriter.Write(Encoding.UTF8.GetString(buffer, offset, count));
            _bufferStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            try
            {
                using (var tidy = new HtmlTidy())
                {
                    var cleanedHtml = tidy.CleanHtml(_bufferWriter.ToString(), HtmlTidyOptions.ConvertToXhtml);
                    cleanedHtml = _generatorRemoval.Replace(cleanedHtml, "");
                    var cleanedHtmlBytes = Encoding.UTF8.GetBytes(cleanedHtml);
                    _httpResponseStream.Write(cleanedHtmlBytes, 0, cleanedHtmlBytes.Length);
                }
            }
            catch
            {
                _bufferStream.WriteTo(_httpResponseStream);
            }
            finally
            {
                _bufferWriter.Close();
                _bufferStream.Close();
            }
        }
    }
}