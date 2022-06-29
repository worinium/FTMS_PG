using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTMS.Common.DTOs
{
    public class FileSticker
    {
        public int sticker_id { get; set; }
        public int file_id { get; set; }
        public string file_number { get; set; }
        public string owner_name { get; set; }
        public string qr_label_txt { get; set; }
        public DateTime created_date { get; set; }
        public string logged_user { get; set; }
        public Boolean active { get; set; }
    }
}
