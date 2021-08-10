﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaCore.Interfaces
{
    public interface IPasswordService 
    {
        string Hash(string password);

        bool Check(string hash, string password);
    }
}
