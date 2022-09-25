namespace TAB2.Api.Interaction;

public struct MultiStringHash
{
    private readonly int hash;

    public MultiStringHash(params string[] strings)
    {
        int result = strings[0].GetHashCode();
        for (int i = 1; i < strings.Length; i++)
        {
            result = HashCode.Combine(result, strings[i].GetHashCode());
        }

        hash = result;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}