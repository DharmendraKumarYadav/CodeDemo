using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public static class ValidationConstants
    {
        public const int TinyTextLength = 32;
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "short")]
        public const int ShortTextLength = 64;
        public const int NormalTextLength = 128;
        public const int BigTextLength = 256;
        public const int LargeTextLength = 512;

        public const int CurrencyCodeLength = 3;

        public const string DateFormat = "dd/MM/yyyy";
        public const string TimeFormat = "hh:mm:ss tt";
        public const string DateTimeFormat = DateFormat + " " + TimeFormat;
        public const string MomentJsDateFormat = "DD/MM/YYYY";

        public const int MinAbsoluteUrlLength = 5;
        public const int MaxAbsoluteUrlLength = 256;
        public const string AbsoluteUrlPattern = "^([hH][tT]|[fF])[tT][pP]([sS]?)\\:\\/\\/(([a-zA-Z0-9\\-\\._]+(\\.[a-zA-Z0-9\\-\\._]+)+)|[lL][oO][cC][aA][lL][hH][oO][sS][tT])(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&amp;%\\$#_]*)?([\\d\\w\\.\\/\\%\\+\\-\\=\\&amp;\\?\\:\\\\\\&quot;\\'\\,\\|\\~\\;]*)$";
       

        public const int NumericGuidLength = 32;
        public const string NumericGuidPattern = @"^[a-fA-F0-9]{32}$";
        

        public const int GuidLength = 38;
        public const string GuidPattern = @"^\{[a-fA-F0-9]{8}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{12}\}$";
        

        public const int MinDisplayNameLength = 2;
        public const int MaxDisplayNameLength = 128;

        public const int MinRoleNameLength = 2;
        public const int MaxRoleNameLength = 64;
        public const string RoleNameRegexPattern = @"^[a-zA-Z][a-zA-Z0-9]*$";
       
        
        public const string NonRoleNameCharsRegexPattern = @"[^a-zA-Z0-9]";
       
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 64;
        public const int PasswordMinLowerAlpha = 1;
        public const int PasswordMinUpperAlpha = 1;
        public const int PasswordMinDigits = 1;
        public const int PasswordMinSpecials = 1;
        public const int PasswordMinUniqueChars = 5;
        public const string PasswordRegexPattern = @"^(?=(.*\d){1,})(?=(.*[a-z]){1,})(?=(.*[A-Z]){1,})(?=(.*\W){1,}).*$";
      

        public const int MinEmailLength = 5;
        public const int MaxEmailLength = 256;
        public const string EmailRegexPattern = "^[a-zA-Z0-9]+([-+._][a-zA-Z0-9]+){0,2}@.*?(\\.([aA](?:[cdefgilmnoqrstuwxzCDEFGILMNOQRSTUWXZ]|([eE][rR][oO])|(?:[rR][pP]|[sS][iI])[aA])|[bB](?:[abdefghijmnorstvwyzABDEFGHIJMNORSTVWYZ][iI][zZ])|[cC](?:[acdfghiklmnoruvxyzACDFGHIKLMNORUVXYZ]|[aA][tT]|[oO](?:[mM]|[oO][pP]))|[dD][ejkmozEJKMOZ]|[eE](?:[ceghrstuCEGHRSTU]|[dD][uU])|[fF][ijkmorIJKMOR]|[gG](?:[abdefghilmnpqrstuwyABDEFGHILMNPQRSTUWY]|[oO][vV])|[hH][kmnrtuKMNRTU]|[iI](?:[delmnoqrstDELMNOQRST]|[nN](?:[fF][oO]|[tT]))|[jJ](?:[emopEMOP]|[oO][bB][sS])|[kK][eghimnprwyzEGHIMNPRWYZ]|[lL][abcikrstuvyABCIKRSTUVY]|[mM](?:[acdeghklmnopqrstuvwxyzACDEGHKLMNOPQRSTUVWXYZ]|[iI][lL]|[oO][bB][iI]|[uU][sS][eE][uU][mM])|[nN](?:[acefgilopruzACEFGILOPRUZ]|[aA][mM][eE]|[eE][tT])|[oO](?:[mM]|[rR][gG])|[pP](?:[aefghklmnrstwyAEFGHKLMNRSTWY]|[rR][oO])|[qQ][aA]|[rR][eosuwEOSUW]|[sS][abcdeghijklmnortuvyzABCDEGHIJKLMNORTUVYZ]|[tT](?:[cdfghjklmnoprtvwzCDFGHJKLMNOPRTVWZ]|(?:[rR][aA][vV])?[eE][lL])|[uU][agkmsyzAGKMSYZ]|[vV][aceginuACEGINU]|[wW][fsFS]|[yY][etuETU]|[zZ][amwAMW])\\b){1,2}$";
       

        public const int MinPhoneNumberLength = 9;
        public const int MaxPhoneNumberLength = 25;
        public const string PhoneNumberRegexPattern = @"^\+[1-9][0-9]*$";
       

        public const double MinLatitude = -90.0;
        public const double MaxLatitude = 90.0;
        public const double MinLongitude = -180.0;
        public const double MaxLongitude = 180.0;

        public static readonly string EmailPhoneNumberRegexPattern = string.Format(CultureInfo.InvariantCulture, "({0})|({1})", EmailRegexPattern, PhoneNumberRegexPattern);
        
        public static readonly Func<DateTime> MinDateOfBirth = () => new DateTime(DateTime.Now.Year - 100, 1, 1);
        public static readonly Func<DateTime> MaxDateOfBirth = () => new DateTime(DateTime.Now.Year - 5, 12, 31);
    }
}
