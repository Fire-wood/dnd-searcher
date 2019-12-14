using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellSearch
{
    public enum School { Abjuration, Conjuration, Divination, Enchantment, Evocation, Illusion, Necromancy, Transmutation, Invalid };
    enum Unit { Action, Instantaneous, Hour, Hours, Minute, Minutes, Reaction, Round, Invalid };
    public enum Classes { Bard, Cleric, Druid, Paladin, Ranger, Sorcerer, Warlock, Wizard, Invalid };
    public enum AoE { None, Cone, Cube, Cylinder, Line, Sphere, Invalid };
    enum Components { Verbal, Somatic, Material, Invalid };
    enum ReferenceBook { PHB, XGtE, SCAG, EE, UA, Invalid };

    public class Spell
    {
        private int _level;
        private int _range; // in feet
        private int _rangeAoE;
        private int _castingTime;
        private int _durationTime;
        private int _bookPage;

        private float _edition;

        public bool _ritual { get; private set; }
        public bool _concentration { get; private set; }

        private Unit _castingUnit;
        private Unit _durationUnit;
        private AoE _aoe;
        private School _school;
        private ReferenceBook _book;
        private List<Classes> _availableClasses;
        private List<Components> _requiredComponents;

        private string _materials;
        private string _description;
        private string _spellName;
        private string _spellHigherDescription;
        private string _dmNote;

        public Spell(string name, string spellFile) {
            this._spellName = name;
            string[] properties = spellFile.Split('\n');

            this._level = Int32.Parse(SearchFor(properties, "level: ", "Unknown Level"));

            bool r;
            Boolean.TryParse(SearchFor(properties, "ritual", ""), out r);
            this._ritual = r;

            bool c;
            Boolean.TryParse(SearchFor(properties, "concentration", ""), out c);
            this._concentration = c;

            DetermineRange(SearchFor(properties, "range: ", "Unknown Range"));
            DetermineCastingTime(SearchFor(properties, "casting: ", "Unknown Casting Time"));
            DetermineDuration(SearchFor(properties, "duration: ", "Unknown Duration"));
            DetermineSchool(SearchFor(properties, "school: ", "Unknown School"));
            DetermineClasses(SearchFor(properties, "classes: ", "Unknown Class"));
            DetermineComponents(SearchFor(properties, "components: ", "Unknown Components"));
            DetermineBook(SearchFor(properties, "book: ", "Unknown Reference Book"));
            DetermineEdition(SearchFor(properties, "edition: ", "Unknown Edition"));
            this._description = SearchFor(properties, "description: ", "Unknown Description");
            this._spellHigherDescription = SearchFor(properties, "higher levels: ", "Unknown Higher Level Description");
            this._dmNote = SearchFor(properties, "dm note: ", "Unknown DM Note");
        }

        #region Getters
        public string GetName()
        {
            return _spellName;
        }

        public int GetLevel()
        {
            return _level;
        }

        public School GetSchool()
        {
            return _school;
        }

        public List<Classes> GetClasses()
        {
            return _availableClasses;
        }

        public string GetBook()
        {
            return this._book.ToString() + " " + this._bookPage.ToString();
        }

        public string GetEdition()
        {
            return this._edition.ToString() + "e";
        }

        public string GetComponents()
        {
            string s = "";
            foreach(Components c in this._requiredComponents)
            {
                s += c.ToString() + ", ";
            }
            s = s.Substring(0, s.Length - 2) + " ";
            return s += this._materials;
        }

        public string GetRange()
        {
            string s = "";
            if(this._range == -2)
            {
                s += "Self";
            } else
            {
                s += this._range.ToString() + " feet";
            }

            if(this._aoe != AoE.None && this._aoe != AoE.Invalid)
            {
                s += " (" + this._rangeAoE + " " + this._aoe.ToString() + ") ";
            }
            return s;
        }

        public string GetCastingTime()
        {
            if(this._castingUnit == Unit.Reaction)
            {
                return Unit.Reaction.ToString();
            } else
            {
                return this._castingTime + " " + this._castingUnit;
            }
        }

        public string GetDuration()
        {
            if(this._durationTime == -1)
            {
                return this._durationUnit.ToString();
            } else
            {
                return this._durationTime + " " + this._durationUnit;
            }
        }

        public string GetDescription()
        {
            return _description;
        }

        public string GetHigherDescription()
        {
            return _spellHigherDescription;
        }

        public string GetDMNote()
        {
            return _dmNote;
        }

        # endregion;

        private string SearchFor(string[] spellProperties, string word, string catchDescription)
        {
            try
            {
                for(int i = 0; i < spellProperties.Length; i++)
                {
                    if (spellProperties[i].ToLower().Trim().Contains(word))
                    {
                        if(word != "ritual" && word != "concentration")
                        {
                            return spellProperties[i].Substring(word.Length);
                        } else
                        {
                            return "true";
                        }
                    } 
                }
                return "";
            }
            catch (Exception)
            {
                Console.WriteLine(catchDescription);
                return "";
            }
        }

        // Determine which school of magic this spells applies to
        private void DetermineSchool(string schoolFile)
        {
            switch(schoolFile.Trim().ToLower()) {
                case "abjuration":
                    this._school = School.Abjuration;
                    break;
                case "conjuration":
                    this._school = School.Conjuration;
                    break;
                case "divination":
                    this._school = School.Divination;
                    break;
                case "enchantment":
                    this._school = School.Divination;
                    break;
                case "evocation":
                    this._school = School.Evocation;
                    break;
                case "illusion":
                    this._school = School.Illusion;
                    break;
                case "necromancy":
                    this._school = School.Necromancy;
                    break;
                case "transmutation":
                    this._school = School.Transmutation;
                    break;
                default:
                    this._school = School.Invalid;
                    Console.WriteLine("Spell has unknown school type");
                    break;
            }
        }

        private void DetermineRange(string rangeFile)
        {
            string[] r = rangeFile.Split(' ');

            if (!Int32.TryParse(r[0], out this._range))
            {
                this._range = -2; // Self
                this._aoe = AoE.None;
            }

            // AoE involved
             if(r.Length == 3)
            {
                this._rangeAoE = Int32.Parse(r[1]);
                switch (r[2].Trim().ToLower())
                {
                    case "cone":
                        this._aoe = AoE.Cone;
                        break;
                    case "cube":
                        this._aoe = AoE.Cube;
                        break;
                    case "cylinder":
                        this._aoe = AoE.Cylinder;
                        break;
                    case "line":
                        this._aoe = AoE.Line;
                        break;
                    case "sphere":
                        this._aoe = AoE.Sphere;
                        break;
                    default:
                        this._aoe = AoE.Invalid;
                        break;
                } 
            } 
        }

        private void DetermineEdition(string editionFile)
        {
            editionFile = editionFile.Trim();
            if(editionFile.Contains("5e") || editionFile.Contains("5"))
            {
                this._edition = 5.0f;
            } else
            {
                this._edition = -1.0f;
                Console.WriteLine("Spell has Unknown Edition");
            }
        }

        private void DetermineBook(string bookFile)
        {
            string[] contents = bookFile.Split(' ');
            int pageNum;
            try
            {
                if (Int32.TryParse(contents[0], out this._bookPage))
                {
                    pageNum = 1;
                }
                else
                {
                    pageNum = 0;
                    int bp;
                    Int32.TryParse(contents[1].Trim(), out bp);
                    this._bookPage = bp;
                }
            }
            catch(Exception e)
            {
                return;
            }

            switch (contents[pageNum].Trim().ToLower())
            {
                case "phb":
                    this._book = ReferenceBook.PHB;
                    break;
                case "xgte":
                    this._book = ReferenceBook.XGtE;
                    break;
                case "scag":
                    this._book = ReferenceBook.SCAG;
                    break;
                case "ee":
                    this._book = ReferenceBook.EE;
                    break;
                case "ua":
                    this._book = ReferenceBook.UA;
                    break;
                default:
                    Console.WriteLine("Unknown Reference Book");
                    this._book = ReferenceBook.Invalid;
                    break;
            }
        }


        // Determine which classes can learn this spell
        private void DetermineClasses(string classesFile)
        {
            _availableClasses = new List<Classes>();
            string[] classesBreakdown = classesFile.Split(',');

            foreach(string c in classesBreakdown)
            {
                switch(c.Trim().ToLower())
                {
                    case "bard":
                        this._availableClasses.Add(Classes.Bard);
                        break;
                    case "cleric":
                        this._availableClasses.Add(Classes.Cleric);
                        break;
                    case "druid":
                        this._availableClasses.Add(Classes.Druid);
                        break;
                    case "paladin":
                        this._availableClasses.Add(Classes.Paladin);
                        break;
                    case "ranger":
                        this._availableClasses.Add(Classes.Ranger);
                        break;
                    case "sorcerer":
                        this._availableClasses.Add(Classes.Sorcerer);
                        break;
                    case "warlock":
                        this._availableClasses.Add(Classes.Warlock);
                        break;
                    case "wizard":
                        this._availableClasses.Add(Classes.Wizard);
                        break;
                    default:
                        this._availableClasses.Add(Classes.Invalid);
                        Console.WriteLine("Spell has unknown class availability");
                        break;
                }
            }
        }

        // Determine what components are required to cast this spell
        private void DetermineComponents(string componentsFile)
        {
            string[] components = componentsFile.Trim().Split(',');
            _requiredComponents = new List<Components>();

            foreach(string s in components)
            {
                switch (s.Trim().ToLower())
                {
                    case "vocal":
                    case "v":
                        _requiredComponents.Add(Components.Verbal);
                        break;
                    case "sematic":
                    case "s":
                        _requiredComponents.Add(Components.Somatic);
                        break;
                    case "material":
                    case "m":
                        _requiredComponents.Add(Components.Material);
                        this._materials = components[components.Count() - 1];
                        return;
                    default:
                        this._requiredComponents.Add(Components.Invalid);
                        break;
                }
            }
        }

        // Determine what the casting time of the spell is
        private void DetermineCastingTime(string castFile)
        {
            string[] casting = castFile.Trim().Split(' ');
            if(casting.Length == 1)
            {
                this._castingTime = -2;
                this._castingUnit = DetermineUnit(casting[0]);
            } else
            {
                this._castingTime = Int32.Parse(casting[0].Trim());
                this._castingUnit = DetermineUnit(casting[1].Trim());
            }
        }

        // Determine what the duration of the spell is
        private void DetermineDuration(string durationFile)
        {
            string[] duration = durationFile.Trim().Split(' ');
            if(!Int32.TryParse(duration[0], out this._durationTime))
            {
                this._durationTime = -1;
                this._durationUnit = DetermineUnit(duration[0]);
            } else
            {
                this._durationUnit = DetermineUnit(duration[1]);
            }


            //this._durationTime = Int32.Parse(duration[0]);
            
        }

        private Unit DetermineUnit(string unit)
        {
            switch(unit.Trim().ToLower())
            {
                case "action":
                    return Unit.Action;
                case "instantaneous":
                    return Unit.Instantaneous;
                case "hour":
                    return Unit.Hour;
                case "hours":
                    return Unit.Hours;
                case "minute":
                    return Unit.Minute;
                case "minutes":
                    return Unit.Minutes;
                case "reaction":
                    return Unit.Reaction;
                case "round":
                    return Unit.Round;
                default:
                    Console.WriteLine("Unknown Unit Type");
                    return Unit.Invalid;
            }
        }
    }
}
