using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit;


namespace FhirFormatFlipper
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private MainViewModel MainViewModel;
    public MainWindow()
    {
      InitializeComponent();
      MainViewModel = new MainViewModel();
      LoadExampleFhirResource();
    }

    private void TextboxLeft_TextChanged(object sender, EventArgs e)
    {
      TextboxRight.TextChanged -= TextboxRight_TextChanged;
      RunConversion(TextboxLeft, TextboxRight);
      TextboxRight.TextChanged += TextboxRight_TextChanged;
    }

    private void TextboxRight_TextChanged(object sender, EventArgs e)
    {
      TextboxLeft.TextChanged -= TextboxLeft_TextChanged;
      RunConversion(TextboxRight, TextboxLeft);
      TextboxLeft.TextChanged += TextboxLeft_TextChanged;

    }

    private void RunConversion(TextEditor FromEditor, TextEditor ToEditor)
    {
      var FormateType = ConverterEngine.IsFormat(FromEditor.Text);
      if (FormateType == ConverterEngine.FhirFormatType.json)
      {
        FromEditor.SetSyntaxType(AvalonEditSyntaxTypes.Json);
        ToEditor.SetSyntaxType(AvalonEditSyntaxTypes.Xml);
      }
      else if (FormateType == ConverterEngine.FhirFormatType.xml)
      {
        FromEditor.SetSyntaxType(AvalonEditSyntaxTypes.Xml);
        ToEditor.SetSyntaxType(AvalonEditSyntaxTypes.Json);
      }
      else
      {
        FromEditor.SetSyntaxType(AvalonEditSyntaxTypes.None);
        ToEditor.SetSyntaxType(AvalonEditSyntaxTypes.None);
      }

      MainViewModel.TextRight = ConverterEngine.FormatConvert(FromEditor.Text);
      ToEditor.Text = MainViewModel.TextRight;
    }

    private void LoadExampleFhirResource()
    {
      string DefaultText = "Place a XML or JSON FHIR resource here.";
      string SoultionPath = string.Empty;
      if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
      {
        SoultionPath = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.DataDirectory;
      }
      else
      {
        SoultionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
      }

      try
      {
        TextboxLeft.Text = System.IO.File.ReadAllText(SoultionPath + @"\FhirResources\ExamplePatient.xml");
      }
      catch
      {
        TextboxLeft.Text = DefaultText;
      }
    }
  }
}
