
using NetResources;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

ReadXmlResource();

//ReadResourceFile(@"C:\Temp\Resource1.resources");

//if (IsValidImage(Resource1.key))
//{
//    Console.WriteLine("Key is Image");
//}

//Console.WriteLine(Resource1.String1);

void ReadXmlResource()
{
    ReadResourceFile("XMLResource.xml");
}

void WriteResourceData()
{
    using ResourceWriter resourceWriter = new ResourceWriter("Resource2.rescources");
    byte[] bytes = File.ReadAllBytes("JsonFile.json");

    resourceWriter.AddResource("username", "Max");
    resourceWriter.AddResourceData("JsonFile", "System.Byte[]", bytes);
    resourceWriter.Generate();
    resourceWriter.Close();

    using FileStream fileStream = File.OpenRead("Resource2.rescources");
    ResourceReader resourceReader = new ResourceReader(fileStream);

    resourceReader.GetResourceData("JsonFile", out var resourceType, out var resourceData);

    var str = System.Text.Encoding.UTF8.GetString(resourceData);

    Console.WriteLine(str);
}
void ReadResourceFile(string filePath)
{
    using FileStream fileStream = File.OpenRead(filePath);

    ResourceReader reader = new ResourceReader(fileStream);

    var enumarator = reader.GetEnumerator();

    while (enumarator.MoveNext())
    {
        string name = enumarator.Key.ToString();
        object value = enumarator.Value;

        Console.WriteLine(name + ": " + value);
    }
}


bool IsValidImage(byte[] bytes)
{
    try
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            Image.FromStream(ms);
        }
    }
    catch (Exception)
    {
        return false;
    }

    return true;
}


void GetResources()
{
    Assembly assembly = Assembly.GetExecutingAssembly();
    string[] resources = assembly.GetManifestResourceNames();

    foreach (string resource in resources)
    {
        Console.WriteLine(resource);
    }
}

void GetResourceByName(string resourceName, Assembly assembly)
{
    using Stream stream = assembly.GetManifestResourceStream(resourceName);

    ResourceReader reader = new ResourceReader(stream);

    foreach (DictionaryEntry entry in reader)
    {
        Console.WriteLine(entry.Key);
        Console.WriteLine(entry.Value);
    }
}

static void LocalizedResources(string[] args)
{
    ResourceManager resourceManager = new ResourceManager("NetResources.Strings", Assembly.GetExecutingAssembly());

    if (args.Length > 0)
    {
        CultureInfo culture = new CultureInfo(args[0]);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    Console.WriteLine(resourceManager.GetString("Hello"));
}