﻿namespace TAB2.Api.Command;

public class UserArgumentInfo : ArgumentInfo
{
    public UserArgumentInfo(string name, string description, bool isRequired = true) : base(name, description, isRequired)
    {
    }
}