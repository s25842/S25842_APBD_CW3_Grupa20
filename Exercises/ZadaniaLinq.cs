using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        //query syntax
        var query = from s in DaneUczelni.Studenci
            where s.Miasto.Equals("Warsaw")
                select  $"{s.NumerIndeksu} , {s.Imie} , {s.Nazwisko} , {s.Miasto}";
                //typ anonimaowy to kompilator na biezaco utworzy tai typ
        return query;
    }
    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        var query = DaneUczelni.Studenci.Select(s => s.Email);
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var query = DaneUczelni.Studenci.OrderBy(s=> s.Nazwisko).ThenBy(s=>s.Imie).Select(s => $"{s.NumerIndeksu}, {s.Imie} {s.Nazwisko}");
        
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var query = DaneUczelni.Przedmioty.Where(s => s.Kategoria.Equals("Analytics"));
        var temp = query.FirstOrDefault();
        
        return (temp is not null) ? [$"{temp.Nazwa},{temp.DataStartu}"] : ["nie ma takiego przedmiotu"] ;
        
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var query = DaneUczelni.Zapisy.Where(s => s.CzyAktywny == true);
        if (!query.Any())
        {
            return ["True"];
        }
        {
            return ["False"];
        }
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var countNotEmptyKatedras = DaneUczelni.Prowadzacy.Count(s => (s.Katedra != null) || !s.Katedra.Equals(string.Empty));
        var query = DaneUczelni.Prowadzacy.Count();

        if (countNotEmptyKatedras != query)
        {
            return ["False"];
        }
        return ["True"];

    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var query = DaneUczelni.Zapisy.Count(s=>s.CzyAktywny == true);
        return [$"{query}"];
        
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var query = DaneUczelni.Studenci.Select(s => s.Miasto).Distinct();
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        var query = DaneUczelni.Zapisy.OrderByDescending(s => s.DataZapisu).
            Select(s => new {s.DataZapisu, s.Id, s.PrzedmiotId}.ToString()).Take(3);
        return query;
    }
    

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var query = DaneUczelni.Przedmioty.OrderBy(s=> s.Nazwa).Skip(2).Take(2).Select(s=> s.Nazwa +" "+ s.Kategoria);
        return query; 
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var query = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id
            , zapis => zapis.StudentId,
            (student, zapis) => new { student.Imie, student.Nazwisko, zapis.DataZapisu }.ToString());
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        var query = DaneUczelni.Zapisy.Join(DaneUczelni.Studenci, zapis => zapis.StudentId,
            student => student.Id, (zapis, student) => new { zapis, student })
            .Join(DaneUczelni.Przedmioty, arg => arg.zapis.PrzedmiotId, przedmiot => przedmiot.Id,
                ((arg1, przedmiot) => new {arg1.student.Imie,arg1.student.Nazwisko,przedmiot.Nazwa}.ToString()));
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var query = DaneUczelni.Zapisy.Join(DaneUczelni.Przedmioty, zapis => zapis.PrzedmiotId,
            przedmiot => przedmiot.Id,
            (Zapis, przedmiot) => new { Zapis, przedmiot }).GroupBy(x => x.przedmiot.Nazwa)
            .Select(x => new {NazwaPrzedmiotu=x.Key, liczbaZapisow = x.Count()}.ToString());
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var query = DaneUczelni.Zapisy.Join(DaneUczelni.Przedmioty, zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id, (zapis, przedmiot) => new { zapis, przedmiot })
            .Where(x => x.zapis.OcenaKoncowa != null).GroupBy(x => x.przedmiot.Nazwa)
            .Select(x => 
                new { Nazwa = x.Key, AVG = x.Average(z => z.zapis.OcenaKoncowa) }.ToString());
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var query = DaneUczelni.Prowadzacy.GroupJoin(DaneUczelni.Przedmioty,
            a => a.Id,
            b => b.ProwadzacyId,
            (a, b) => new { a.Imie, a.Nazwisko, LiczbaPrzedmiotow = b.Count() }.ToString());
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var query = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id,
            zapis => zapis.StudentId,
            (student, zapis) => new { zapis, student })
            .Where(a => a.zapis.OcenaKoncowa != null)
            .GroupBy(a => new{a.student.Imie, a.student.Nazwisko})
            .Select(a=>
                new {a.Key.Imie,a.Key.Nazwisko, MaxOcena = a.Max(o => o.zapis.OcenaKoncowa)}.ToString());
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var query = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id,
            zapis => zapis.StudentId,
            (student, zapis) => new { zapis, student })
            .Where(a => a.zapis.CzyAktywny)
            .GroupBy(x => new { x.student.Imie, x.student.Nazwisko })
            .Where(a => a.Count() > 1)
            .Select(x=> new{x.Key.Imie, x.Key.Nazwisko, Aktywne = x.Count()}.ToString());
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var query = DaneUczelni.Przedmioty.Join(DaneUczelni.Zapisy, przedmioty => przedmioty.Id,
                zapis => zapis.PrzedmiotId,
                ((przedmiot, zapis) => new { przedmiot, zapis }))
            .Where(x => x.przedmiot.DataStartu.Month == 4 && x.przedmiot.DataStartu.Year == 2026)
            .GroupBy(x => x.przedmiot.Nazwa)
            .Where(x => x.All(z => z.zapis.OcenaKoncowa == null))
            .Select(x => x.Key.ToString());
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var query = DaneUczelni.Prowadzacy.GroupJoin(DaneUczelni.Przedmioty,
                a => a.Id,
                b => b.ProwadzacyId,
                (a, b) => new { a, b })
            .SelectMany(
                x => x.b.DefaultIfEmpty(),
                (x, b) => new { x.a, b }
            )
            .GroupJoin(DaneUczelni.Zapisy,
                c => c.a.Id,
                d => d.PrzedmiotId,
                (c, d) => new { c, d })
            .SelectMany(
                x => x.d.DefaultIfEmpty(),
                (x, y) => new { x.c, x.c.b, y }
            ).Where(x => x.y != null && x.y.OcenaKoncowa.HasValue)
            .GroupBy(x => new { x.c.a.Imie, x.c.a.Nazwisko })
            .Select(x =>
                new { x.Key.Imie, x.Key.Nazwisko, srednia = x.Average(x => x.y.OcenaKoncowa.Value) }.ToString());
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var query = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id,
                    zapis => zapis.StudentId,
                    (student, zapis) => new { student, zapis })
                .Where(z => z.zapis.CzyAktywny)
                .GroupBy(x => x.student.Miasto)
                .OrderByDescending(z => z.Count())
                .Select(x => new { Miasto = x.Key, count = x.Count() }.ToString())
            ;
        return query;
                
        
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    { 
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
