using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class EncyclopediaItem : MonoBehaviour
{
    [SerializeField, TextArea] private string nameString;
    [SerializeField, TextArea(0,45)] private string descriptionString;
    [SerializeField] private Sprite sprite;

    private Button button;

    [Header("Event Settings: ")]
    [SerializeField] private bool isEventItem = false;
    [SerializeField] private Image eventImage;
    [SerializeField] private TextMeshProUGUI eventNameText;

    private Dictionary<string, string> possibleDescriptions = new Dictionary<string, string>();

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowInEcyclopedia);

        if (isEventItem) { PopulatePossibleDiscriptions(); }
    }

    public void ShowInEcyclopedia()
    {
        GameManager.Instance.UIManager.Encyclopedia.Activate(nameString, descriptionString, sprite, isEventItem);
    }

    public void SetValuesAndCheckIfShouldBeEnabled()
    {
        if (isEventItem)
        {
            sprite = eventImage.sprite;
            nameString = eventNameText.text.Trim(':');
            descriptionString = CheckIfDescriptionExists(nameString);

            gameObject.SetActive(descriptionString != null);
        }
    }

    private string CheckIfDescriptionExists(string _name)
    {
        foreach (var _desc in possibleDescriptions)
        {
            if(_desc.Key == _name) { return _desc.Value; }
        }

        return null;
    }

    private void PopulatePossibleDiscriptions()
    {
        possibleDescriptions.Add("Johannes van den Bosch",
            "Johannes wordt geboren in Herwijnen op 2 februari 1780. Door zijn vader – een dorpsarts – wordt hij ingeschreven als leerling " +
            "apotheker, maar hij wil zelf graag militair worden. Op zijn zestiende gaat hij het leger in en maakt razendsnel carrière in Batavia, " +
            "waar hij trouwt met Catharina de Sandol Roy." +
            "\n \n" +
            "Nadat zijn schoonvader ruzie krijgt met de nieuwe gouverneur-generaal, vraagt Johannes om " +
            "eervol ontslag uit het leger. Hij vertrekt in 1810 naar Nederland, maar wordt gevangen genomen door een Engels oorlogsschip. Twee jaar " +
            "later wordt hij vrijgelaten. Hij verjaagt troepen van Napoleon uit Utrecht en Naarden. Zo wordt hij vertrouweling van Koning Willem I. " +
            "\n \n" +
            "In 1818 richt hij de Maatschappij van Weldadigheid op en start met de bouw van armenkoloniën. Enkele jaren later volgen de strafkolonie " +
            "Ommerschans en bedelaars- en wezengesticht Veenhuizen. In 1827 wordt hij naar West-Indië gezonden om de overzeese koloniën te verbeteren. " +
            "\n \n" +
            "Een jaar later gaat hij naar Oost-Indië en stelt daar het Cultuurstelsel in. Terug in Nederland (1834) moet hij als minister van Koloniën" +
            " de winsten uit Nederlands-Indië vergroten. Enkele jaren later treed hij af, wordt lid van de Tweede Kamer en overlijdt in 1844 na een " +
            "kort ziekbed. ");



        possibleDescriptions.Add("Gouverneur Hofstede",
            "Hofstede is de gouverneur van Drenthe. Hij heeft de bijnaam ‘koning van Drenthe’ en staat erom bekend baantjes en voordelen aan " +
            "familie en vrienden te geven. Als hij in 1818 hoort dat <color=#3b76a3>Johannes van den Bosch</color> land zoekt voor de Kolonie, weet hij nog wel iets. " +
            "Zijn zoon heeft een waardeloos stuk land gekocht voor 30.000 gulden. Nieuwe vraagprijs: 130.000 gulden. <color=#3b76a3>Petrus Ameshoff</color>" +
            " en <color=#3b76a3>Stephanus van Royen</color> weten dit op het nippertje te voorkomen.");



        possibleDescriptions.Add("Commissielid Ameshoff",
            "Petrus Ameshoff is het jongste lid van de Commissie van Weldadigheid. Hij wordt ook kasbewaarder van de Permanente Commissie. " +
            "Hij staat bekend als kritisch en hardwerkend. Hij komt veel in Drenthe en zorgt samen met <color=#3b76a3>Stephanus van Royen</color> dat de Proefkolonie " +
            "vlakbij Vledder komt te liggen. Omdat hij zakenman in Amsterdam is, speelt hij een belangrijke rol in het verschepen van extra mest " +
            "en voedsel naar de Koloniën. Daarnaast wordt hij constant lastiggevallen door armen, die ook in de Kolonie geplaatst willen worden." +
            " Regelmatig draagt hij een kolonist voor, vaak met twijfelachtig resultaat. Veel kolonisten blijken ongeschikt voor het harde werk en " +
            "de strenge regels. Als in 1822 de plannen voor Veenhuizen worden gepresenteerd, is Ameshoff zeer kritisch. Hij vindt dat de Maatschappij " +
            "van Weldadigheid geen ervaring heeft met opvang van weeskinderen en hiermee niet mag experimenten. Er wordt niet naar hem geluisterd.");



        possibleDescriptions.Add("Benjamin van den Bosch",
            "Benjamin is het tien jaar jongere broertje van <color=#3b76a3>Johannes</color>. Hij volgt ook een carrière in het leger. In 1818 wordt hij directeur van de " +
            "Koloniën van Weldadigheid. Hij gaat er wonen en ontvangt samen met zijn broer de eerste vijf gezinnen in december van dat jaar. " +
            "Er is dan nog verder niemand. In de maanden daarna wordt hij geholpen door vier lagere officieren. Die functie wordt later vervangen " +
            "door <color=#3b76a3>wijkmeesters</color>." +
            "\n \n" +
            "Benjamin kent de kolonisten bij naam en trekt zich hun lot aan. Hij schrijft regelmatig brieven aan zijn broer, waarin hij zijn " +
            "zorgen uit over kolonisten die zich misdragen of te weinig hebben om van te leven. Deze betrokkenheid wordt hem fataal. In 1821 " +
            "neemt hij ontslag, nadat het complot van <color=#3b76a3>Johan Bosch</color> en <color=#3b76a3>Hendrik Vos</color> ontrafeld is. Zijn directe opvolger is Wouter Visser, die meteen" +
            " met harde maatregelen komt. De derde directeur van de Koloniën is <color=#3b76a3>Jan van Konijnenburg</color>.");



        possibleDescriptions.Add("Schout van Royen",
            "Stephanus van Royen is vanaf 1795 schout/burgemeester van Vledder. Hij wordt de belangrijkste vriend van de Maatschappij van " +
            "Weldadigheid. In 1818 zorgt hij ervoor dat de Proefkolonie naast zijn dorp komt te liggen, in plaats van op het slechtere land " +
            "bij Witten. Het komt daarbij goed uit dat hij ook notaris is: ook in de jaren daarna gaat hij met Johannes mee op zoek naar extra " +
            "land. " +
            "\n \n" +
            "Ook kent hij alle boeren in de wijde omgeving, waardoor het lukt om ze een dag gratis het land te laten omploegen. " +
            "Zo is de Proefkolonie goed voorbereid op de onervaren stedelingen. Hij schrijft positieve stukken in de krant en bemiddelt " +
            "bij conflicten. " +
            "\n \n" +
            "Zijn broer Jacobus is minder positief. Als Johannes in 1819 koloniehuizen laat bouwen op de heide, is Jacobus van Royen " +
            "de leider van een groep saboteurs. Deze boeren vinden dat de heide hun toebehoort en vernielen de funderingen. Uiteindelijk " +
            "lost <color=#3b76a3>Johannes</color> dit <i>Vledderheide-incident</i> op door het land gewoon te kopen.");



        possibleDescriptions.Add("Dhr. Nobel, eigenaar landgoed Westerbeeksloot",
            "Deze oud-burgemeester van Ruinen speelt een bijrol tijdens de oprichting van de Proefkolonie. Het landgoed Westerbeeksloot " +
            "is in 1818 namelijk van hem. Slechts een jaar daarvoor heeft hij het land gekocht van de familie Van Heloma, die het zelf in " +
            "1766 in bezit had gekregen van de familie Van Westerbeek. " +
            "\n \n" +
            "Nobel woont zelf in Havezate Oldenhave (Ruinen) voordat hij dat laat veilen en afbreken. De verkoop van landgoed " +
            "Westerbeeksloot komt hem dan ook slecht uit. Hij gaat uiteindelijk akkoord wanneer wordt gesuggereerd dat hij daarmee de " +
            "Prins der Nederlanden een grote eer zal bewijzen. Het is onduidelijk of de prins ooit zijn naam heeft horen noemen.");



        possibleDescriptions.Add("Directeur Jan van Konijnenburg",
            "Jan van Konijnenburg is de derde directeur van de Koloniën van Weldadigheid. Hij is geboren in 1799 in Noordwijk. " +
            "Zijn voorgangers zijn <color=#3b76a3>Benjamin van den Bosch</color> en Wouter Visser, die kolonie-inspecteur wordt. Van Konijnenburg is " +
            "secretaris van de Permanente Commissie als hij in 1829 tot directeur wordt benoemd. Hij blijft dit dertig jaar lang. " +
            "Vervolgens wordt hij directeur van de Rijksgestichten Ommerschans en Veenhuizen. Hij overlijdt in 1875 in Den Haag. ");



        possibleDescriptions.Add("Spinbaas Anthonie Brouwer",
            "Brouwer is geboren in 1782 in Amsterdam. Hij heeft ervaring met het runnen van een grote spinnerij en wordt daarom " +
            "in 1820 aangesteld als baas van de <color=#3b76a3>spinzaal</color>. Zijn voorgangers zijn amateurs die 7 gulden per week verdienen, " +
            "hijzelf vraagt 500 gulden per jaar. " +
            "\n \n" +
            "In de maanden daarvoor hebben de kolonisten gesjoemeld met het spinwerk om hoger " +
            "loon voor minder werk te krijgen. Brouwer moet orde op zaken stellen: regelmatig weigert hij voor slecht uitgevoerd " +
            "spinwerk te betalen. De kolonisten zijn hem al gauw spuugzat. Bijna de helft beklaagt zich of gaat stappen verder: tot aan " +
            "stakingen en opstand toe. De strenge maatregelen worden wat afgezwakt. Toch blijft de sfeer tussen hem en de kolonisten gespannen. " +
            "Hij vraagt ontslag, maar krijgt een verdubbeling van zijn loon en de titel ‘directeur over alle fabrieksmatigen arbeid’ in " +
            "alle koloniën. Hij doet dat werk twaalf jaar lang.");



        possibleDescriptions.Add("Wijkmeester",
           "Tijdens de eerste jaren wordt de Proefkolonie gerund door de directeur, samen met enkele sergeanten. Maar de nieuwe Kolonie " +
           "Willemsoord geeft problemen. Hier moeten weeskinderen ondergebracht worden bij gezinnen. Maar de subcommissies zijn te " +
           "enthousiast. Op 1 juni 1820 komen bijna 70 weeskinderen aan, terwijl de bouwvakkers nog bezig zijn. Met volgende schepen" +
           " komen daar nog eens 179 weeskinderen bij. En het einde is nog niet in zicht. " +
           "\n \n" +
           "Er zijn te weinig gezinnen meegestuurd die de weeskinderen kunnen opvangen. De directeur moet zelf bij boerderijen " +
           "langs om onderdak voor ze te vragen. Dat lukt, maar voortaan deelt hij de Koloniën in wijken in, met een wijkmeester als " +
           "hoofd. Wat deze doet, verandert in de loop der tijd. Zo controleren ze om de dag de huizen van alle kolonisten op netheid. " +
           "Ze straffen spijbelende leerlingen. Ze treden op tegen kolonisten die tegen de regels in een hond als huisdier hebben. ");



        possibleDescriptions.Add("Kolonist Johan Bosch",
           "Johannes Bosch is de oudste kolonist in de eerste Kolonie. Hij komt uit Amsterdam en heeft drie dochters, allemaal getrouwd. " +
           "Hij neemt twee dochters en schoonzonen mee naar de Kolonie. Maar hij brengt de verkeerde. Hij krijgt al gauw bijnamen als ‘schurk’. " +
           "\n \n" +
           "Hij moppert veel en wordt ervan verdacht zijn geld te verkwisten. De druppel is dat de directie hem ongeschikt vindt voor landarbeid: " +
           "hij moet voortaan spinnen. Hij vindt dat broodroof en begint stiekem kritische brieven te schrijven aan subcommissie en permanente " +
           "commissie. Daarnaast werkt hij samen met <color=#3b76a3>Hendrik Vos</color> aan een plan om gevangenen te bevrijden en een opstand te beginnen. " +
           "\n \n" +
           "Uiteindelijk komt dit plan uit. Johannes wordt naar de strafkolonie Ommerschans gestuurd, samen met een deel van zijn familie. " +
           "Ook daar schrijft hij weer brieven. Dat wordt ontdekt en Johannes moet nog 6 jaar in de strafkolonie verblijven. Het is onbekend " +
           "hoe het verder met hem afloopt.");



        possibleDescriptions.Add("Kolonist Hendrik Vos",
           "Hendrik Vos uit Maassluis is een van de eerste kolonisten. Hij staat al gauw bekend als ongehoorzaam en lui. Dat leidt " +
           "tot lagere lonen. Zijn ontevreden vrouw vertrekt zonder toestemming en wordt opgepakt. Als zij in het strafhok zit, " +
           "bedenkt Hendrik een plan om haar met geweld te bevrijden. Hij zoekt hulp en vindt die onder andere bij <color=#3b76a3>Johannes Bosch</color>. " +
           "\n \n" +
           "Met enkele andere kolonisten schrijven ze kritische brieven. Ook bedenken ze een plan voor een opstand of een aanslag " +
           "op de directeur. Zo ver komt het niet. Het plan wordt ontdekt en Hendrik wordt als leider aangewezen. Hij wordt meteen " +
           "verbannen, hoewel de subcommissie protesteert. Hebben ze hiervoor contributie betaald?");




        possibleDescriptions.Add("Koning Willem I",
           "Willem Frederik van Oranje Nassau is de zoon van Stadhouder Willem V. Hij wordt geboren in 1772 in Berlijn, want " +
           "zijn moeder is een prinses uit Pruisen. Hij trouwt met zijn nicht en krijgt vijf kinderen, waaronder de toekomstige " +
           "koning Willem II." +
           "\n \n" +
           "Vanaf 1793 vecht hij tegen de Fransen, maar twee jaar later moet hij samen met zijn vader voor ze vluchten " +
           "naar Engeland. Hij onderhandelt met Napoleon over het terugkrijgen van zijn bezittingen. Dat lukt deels, maar hij wordt " +
           "vazal van de keizer. Als Napoleon Pruisen aanvalt, kiest Willem voor zijn familie. Napoleon neemt voor straf zijn bezittingen in. " +
           "\n \n" +
           "In 1813 is Willem eindelijk weer in Nederland, met een uitnodiging van Nederlandse hooggeplaatsten om koning te worden. Napoleon is " +
           "dan verslagen en bezet de Nederlanden niet meer. Het Congres van Wenen besluit dat er een sterk land ten noorden van Frankrijk moet " +
           "komen. Nederland, België en Luxemburg worden samengevoegd tot het Verenigd Koninkrijk der Nederlanden. Willem I is de koning van een " +
           "land dat door oorlogen en belastingen zeer arm is geworden. Hij laat wegen, kanalen, spoorlijnen en bruggen aanleggen. Als " +
           "Johannes met het plan komt armen boer te laten worden, is Willem I graag bereid hem te helpen.");
    }
}