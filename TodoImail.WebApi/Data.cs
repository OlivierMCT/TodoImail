using System.Drawing;
using TodoImail.Services.Entities;

namespace TodoImail.WebApi {
    public class Data {
        internal static IEnumerable<CategoryEntity> GetCategories() {
            List<string> labels = ["pro", "perso", "apéro", "vacances", "projet"];
            return labels.Select(l => new CategoryEntity() { 
                Color = Color.FromArgb(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256)).ToArgb() + "",
                Label = l,
            });
        }

        internal static IEnumerable<TodoEntity> GetTodos(List<CategoryEntity> categoryEntities) {
            var geoData = new Dictionary<string, (double Latitude, double Longitude)?> {
            { "Marcher sur la Muraille de Chine", (40.4319, 116.5704) },
            { "Faire un jogging dans Central Park", (40.785091, -73.968285) },
            { "Voir le Grand Canyon", (36.106965, -112.112997) },
            { "Voir les pyramides en Égypte", (29.9792, 31.1342) },
            { "Voir le Taj Mahal", (27.1751, 78.0421) },
            { "Voir les chutes du Niagara", (43.0962, -79.0377) },
            { "Voir les statues de l’île de Pâques", (-27.1127, -109.3497) },
            { "Naviguer sur la baie d’Halong au Vietnam", (20.9101, 107.1839) },
            { "Voir Pétra en Jordanie", (30.3285, 35.4444) },
            { "Gravir le Mont Fuji", (35.3606, 138.7274) },
            { "Voir le Salar d’Uyuni en Bolivie", (-20.1338, -67.4891) },
            { "Voir les chutes d’Iguazu", (-25.6953, -54.4367) },
            { "Voir le rocher d’Uluru en Australie", (-25.3444, 131.0369) },
            { "Aller à Ushuaïa, la ville la plus australe du monde", (-54.8019, -68.3030) },
            { "Voir des kangourous", (-25.2744, 133.7751) },
            { "Voir un dragon de Komodo", (-8.5500, 119.4500) },
            { "Voir les temples d’Angkor Wat au Cambodge", (13.4125, 103.8667) },
            { "Voir de la lave en fusion sur un volcan", (19.421, -155.287) },
            { "Faire le trek de l’Inca jusqu’au Machu Picchu", (-13.1631, -72.5450) },
            { "Aller sur les îles Galapagos", (-0.9538, -90.9656) },
            { "Marcher dans Sequoia National Park en Californie", (36.4864, -118.5658) },
            { "Prendre le tram à San Francisco", (37.7749, -122.4194) },
            { "Monter en haut de l’Empire State Building", (40.748817, -73.985428) },
            { "Faire la full moon party à Koh Panghan en Thaïlande", (9.7319, 100.0136) },
            { "Marcher dans le Sahara", (23.4162, 25.6628) },
            { "Faire la route des vins en Argentine", (-32.8895, -68.8458) }, // Mendoza
            { "Faire un safari", (-1.2921, 36.8219) }, // Nairobi, Kenya
            { "Parcourir la forêt amazonienne en pirogue", (-3.4653, -62.2159) }, // Amazonie, Brésil
            { "Nager à Palawan aux Philippines", (9.8349, 118.7384) },
            {"Corriger le bug #43587", null },
            {"Supprimer le thême 'Blink'", null },
            {"Ajouter un bouton 'Like'", null },
            {"Demander une augmentation", null },
            {"Poser des congés", null },
            {"Se préparer à l'entretien annuel", null },
            {"S'inscrire à la formation Angular notions avancées", null },
            {"Corriger le bug #98256", null },
            {"Installer Visual Studio 2019", null },
            {"Ajouter les Tests Unitaires", null },
            {"Supprimer tous les icones du bureau", null },
            {"Configurer GIT", null },
            {"Monter en compétences sur .net core", null },
            {"Installer Bootstrap 4", null },
            {"Apporter les chocolatines", null },
            {"Appeler le Helpdesk", null },
            {"Finir la documentation", null },
            {"Corriger le bug #12856", null },
            {"Installer un antivirus", null },
            {"Se désincrire des newsletter", null },
            {"Corriger le bug #22384", null },
            {"Reinstaller Windows 10", null },
            {"Préparer les vacances", null },
            {"Changer le fond d'écran", null },
            {"Améliorer les performances de l'app iPhone", null },
            {"Corriger le bug #56812", null },
            {"Regarder MongoDB", null },
            {"Visualiser la dernière conférence NG", null },
            {"Corriger le bug #45098", null },
            {"Changer la photo de profil", null },
            {"Monter le Kilimandjaro", (-3.0674, 37.3556) },
            {"Nager avec un requin en liberté", (-17.7134, 178.0650) }, // Fidji
            {"Poser les pieds sur un des pôles du globe", (-90.0, 0.0) }, // Pôle Sud
            {"Manger du cochon d’inde au Pérou", (-13.5319, -71.9675) }, // Cusco, Pérou
            {"Faire le tour de la Patagonie", (-51.7290, -72.5026) }, // El Calafate, Argentine
            {"Aller voir un match de foot au Brésil", (-22.9068, -43.1729) }, // Rio de Janeiro
            {"Voir une éclipse", null },
            {"Apprendre le yoga", (28.5858, 77.2090) }, // Rishikesh, Inde
            {"Avoir un tampon sur toutes les pages de mon passeport", null },
            {"Descendre la route de la mort en VTT en Bolivie", (-16.3086, -68.0282) }, // Yungas Road
            {"Participer au carnaval de Rio", (-22.9068, -43.1729) },
            {"Passer au-dessus de la barre des 5000 mètres d’altitude", (27.9881, 86.9250) }, // Everest
            {"Voir une aurore boréale", (69.6496, 18.9560) }, // Tromsø, Norvège
            {"Faire une balade en montgolfière", (29.9792, 31.1342) }, // Pyramides d'Égypte (spot connu)
            {"Sauter à l’élastique", (45.9237, 6.8694) }, // Pont de l'Artuby, France
            {"Faire de la plongée sous-marine", (13.4443, 144.7937) }, // Guam
            {"Boire du sang de serpent", (21.0285, 105.8542) }, // Hanoï, Vietnam
            {"Faire du rafting", (27.7172, 85.3240) }, // Katmandou, Népal
            {"Voir des geisers", (64.3104, -20.3024) }, // Geysir, Islande
            {"Vivre plusieurs mois dans un pays étranger", null },
            {"Voir une baleine", (64.9631, -19.0208) }, // Islande
            {"Prendre le transsibérien", (55.7558, 37.6173) }, // Moscou
            {"Faire l’amour sur une plage", null },
            {"Parcourir les route 66 aux États-Unis", (35.1983, -111.6513) }, // Flagstaff, Arizona
            {"Faire un saut en parachute", (48.8588, 2.3470) }, // Paris (générique)
            {"Faire un road trip en van en Australie", (-25.2744, 133.7751) },
            {"Faire du kitesurf", (27.9158, 34.3299) }, // Dahab, Égypte
            {"Parcourir les rizières à vélo", (20.9214, 105.8572) }, // Ninh Binh, Vietnam
            {"Passer une nuit sans voir le soleil se coucher près du cercle polaire", (69.6496, 18.9560) }, // Tromsø, Norvège
            {"Écrire un livre", null },
            {"Apprendre le tango à Buenos Aires", (-34.6037, -58.3816) },
            {"Apprendre la salsa", (23.1136, -82.3666) }, // La Havane, Cuba
            {"Faire du snorkeling sur la Grande Barrière de Corail", (-18.2871, 147.6992) },
            {"Faire l’ascension d’un volcan", (19.421, -155.287) }, // Hawaï
            {"Naviguer sur la rivière souterraine de Puerto Princesa aux Philippines", (10.1960, 118.9262) },
            {"Discuter avec un moine tibétain", (29.6536, 91.1172) }, // Lhassa, Tibet
            {"Embrasser quelqu’un sous la pluie", null },
            {"Jouer comme figurant dans un film de Bollywood en Inde", (19.0760, 72.8777) }, // Mumbai
            {"Prendre un bain de minuit", null },
            {"Gravir une pyramide Aztèque ou Maya", (20.6829, -88.5686) }, // Chichen Itza, Mexique
            {"Faire du wwoofing", null },
            {"Dormir à la belle étoile dans le désert", (23.4162, 25.6628) }, // Sahara
            {"Faire du surf", (-23.5338, -46.6253) }, // Sao Paulo, Brésil
            {"Faire cracher un lama", (-13.5319, -71.9675) }, // Cusco, Pérou
            {"Faire trek de plusieurs semaines au Népal", (28.3949, 84.1240) },
            {"Faire de l’auto-stop", null },
            {"Dormir dans une maison sur pilotis", (0.7893, 113.9213) }, // Indonésie (générique)
            {"Rester éveillé toute la nuit pour voir le lever du soleil", null },
            {"Traverser un pont de singe au-dessus d’un grand précipice", (27.9881, 86.9250) }, // Himalaya
            {"Créer mon blog", null },
            {"Apprendre à jouer d’un instrument", null },
            {"Traverser un océan en voilier", (36.7213, -4.4214) }, // Malaga, Espagne (port de départ typique)
            {"Traverser l’équateur", (0.0, 39.6667) }, // Kenya
            {"Parcourir une longue distance en autostop", null },
            {"Apprendre une nouvelle langue étrangère", null },
            {"Tomber amoureux(se)", null },
            {"Faire une retraite bouddhiste", null },
            {"Faire un voyage seul", null },
            {"Se baigner sous une cascade", (14.1790, 99.3086) }, // Erawan Falls, Thaïlande
            {"Faire du chien de traîneau", (69.6496, 18.9560) }, // Tromsø, Norvège
            {"Ne pas voir l’hiver pendant un an en changeant d’hémisphère", null },
            {"Jouer au poker à Las Vegas", (36.1699, -115.1398) },
            {"Voir un match de cricket en Inde", (19.0760, 72.8777) }, // Mumbai
            {"Grimper jusqu’au Christ Rédempteur à Rio", (-22.9519, -43.2105) },
            {"Manger un insecte", (13.7563, 100.5018) }, // Bangkok, Thaïlande
            {"Faire du parapente", (45.8992, 6.1296) }, // Annecy, France
            {"Avoir ma photo dans un journal étranger", null },
            {"Faire un câlin à un koala", (-27.4698, 153.0251) }, // Brisbane, Australie
            {"Voir un match de boxe thaï", (13.7563, 100.5018) }, // Bangkok
            {"Nager avec des tortues", (-0.4296, 166.9386) }, // Nauru
            {"Me baigner dans un piscine d’eau chaude naturelle", (64.8631, -16.8065) }, // Blue Lagoon, Islande
        };
            return geoData.Select(gd => new TodoEntity() {
                CategoryId = Random.Shared.Next() % 5 == 0 ? null : categoryEntities[Random.Shared.Next(0, categoryEntities.Count)].Id,
                DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(Random.Shared.Next(-60, 90))),
                IsDone = Random.Shared.Next() % 3 == 0,
                Label = gd.Key,
                Latitude = gd.Value?.Latitude,
                Longitude = gd.Value?.Longitude
            });
        }
    }
}
