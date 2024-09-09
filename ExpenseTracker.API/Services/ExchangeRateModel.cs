using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpenseTracker.API.Services;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks />
[SerializableAttribute]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.bnr.ro/xsd")]
[XmlRoot(Namespace = "http://www.bnr.ro/xsd", IsNullable = false)]
public class DataSet
{
  private DataSetBody bodyField;
  private DataSetHeader headerField;

  /// <remarks />
  public DataSetHeader Header
  {
    get => headerField;
    set => headerField = value;
  }

  /// <remarks />
  public DataSetBody Body
  {
    get => bodyField;
    set => bodyField = value;
  }
}

/// <remarks />
[SerializableAttribute]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.bnr.ro/xsd")]
public class DataSetHeader
{
  private string messageTypeField;
  private string publisherField;

  private DateTime publishingDateField;

  /// <remarks />
  public string Publisher
  {
    get => publisherField;
    set => publisherField = value;
  }

  /// <remarks />
  [XmlElement(DataType = "date")]
  public DateTime PublishingDate
  {
    get => publishingDateField;
    set => publishingDateField = value;
  }

  /// <remarks />
  public string MessageType
  {
    get => messageTypeField;
    set => messageTypeField = value;
  }
}

/// <remarks />
[SerializableAttribute]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.bnr.ro/xsd")]
public class DataSetBody
{
  private DataSetBodyCube cubeField;

  private string origCurrencyField;
  private string subjectField;

  /// <remarks />
  public string Subject
  {
    get => subjectField;
    set => subjectField = value;
  }

  /// <remarks />
  public string OrigCurrency
  {
    get => origCurrencyField;
    set => origCurrencyField = value;
  }

  /// <remarks />
  public DataSetBodyCube Cube
  {
    get => cubeField;
    set => cubeField = value;
  }
}

/// <remarks />
[SerializableAttribute]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.bnr.ro/xsd")]
public class DataSetBodyCube
{
  private DateTime dateField;
  private DataSetBodyCubeRate[] rateField;

  /// <remarks />
  [XmlElement("Rate")]
  public DataSetBodyCubeRate[] Rate
  {
    get => rateField;
    set => rateField = value;
  }

  /// <remarks />
  [XmlAttribute(DataType = "date")]
  public DateTime date
  {
    get => dateField;
    set => dateField = value;
  }
}

/// <remarks />
[SerializableAttribute]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.bnr.ro/xsd")]
public class DataSetBodyCubeRate
{
  private string currencyField;

  private byte multiplierField;

  private bool multiplierFieldSpecified;

  private decimal valueField;

  /// <remarks />
  [XmlAttribute]
  public string currency
  {
    get => currencyField;
    set => currencyField = value;
  }

  /// <remarks />
  [XmlAttribute]
  public byte multiplier
  {
    get => multiplierField;
    set => multiplierField = value;
  }

  /// <remarks />
  [XmlIgnore]
  public bool multiplierSpecified
  {
    get => multiplierFieldSpecified;
    set => multiplierFieldSpecified = value;
  }

  /// <remarks />
  [XmlText]
  public decimal Value
  {
    get => valueField;
    set => valueField = value;
  }
}