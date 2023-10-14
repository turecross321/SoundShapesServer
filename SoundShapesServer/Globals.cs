namespace SoundShapesServer;

public static class Globals
{
    public const int OneHourInSeconds = 3600;
    public const int FourHoursInSeconds = 14400;
    public const int TenMinutesInSeconds = 600;
    public const int OneDayInSeconds = 86400;
    public const int OneMonthInSeconds = 86400 * 30;

    public const int FourMegabytes = 4000000;
    
    // ReSharper disable once IdentifierTypo
    // ReSharper disable once InconsistentNaming
    public const string AGPLLicense = """
                                       This program is free software: you can redistribute it and/or modify
                                       it under the terms of the GNU Affero General Public License as published
                                       by the Free Software Foundation, either version 3 of the License, or
                                       (at your option) any later version.

                                       This program is distributed in the hope that it will be useful,
                                       but WITHOUT ANY WARRANTY; without even the implied warranty of
                                       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                                       GNU Affero General Public License for more details.

                                       You should have received a copy of the GNU Affero General Public License
                                       along with this program.  If not, see <https://www.gnu.org/licenses/>.
                                       """;
}