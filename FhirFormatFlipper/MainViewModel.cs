using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirFormatFlipper
{
  public class MainViewModel
  {
    private string _TextLeft;
    public string TextLeft
    {
      get { return _TextLeft; }
      set { _TextLeft = value; }
    }

    private string _TextRight;
    public string TextRight
    {
      get { return _TextRight; }
      set { _TextRight = value; }
    }
    
  }
}
