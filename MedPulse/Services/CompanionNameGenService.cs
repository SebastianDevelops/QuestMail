using Syllabore;
using Syllabore.Fluent;

namespace MedPulse.Services;

public static class CompanionNameGenService
{
    public static string GenCompanionName()
    {
        var names = new NameGenerator("str", "aeo");
        return names.Next();
    }
}