using System.ComponentModel;
using System.Reflection;

namespace OpenVisStreamer.VideoLibrary.Model.Entities;

public enum VideoCategory
{
    [Description("Other")]
    Other,
    
    [Description("Fun")]
    Fun,
    
    [Description("Educational")]
    Educational,
    
    [Description("Music")]
    Music,
    
    [Description("News")]
    News,
    
    [Description("Sports")]
    Sports,
     [Description("DIY Projects & Crafts")]
    DIYProjectsAndCrafts,
    
    [Description("Technology & Gadgets Reviews")]
    TechnologyAndGadgetsReviews,
    
    [Description("Cooking & Recipes")]
    CookingAndRecipes,
    
    [Description("Fitness & Exercise Routines")]
    FitnessAndExerciseRoutines,
    
    [Description("Educational Tutorials")]
    EducationalTutorials,
    
    [Description("Travel Vlogs")]
    TravelVlogs,
    
    [Description("Gaming & Esports")]
    GamingAndEsports,
    
    [Description("Fashion & Beauty Tips")]
    FashionAndBeautyTips,
    
    [Description("Music Covers & Originals")]
    MusicCoversAndOriginals,
    
    [Description("Comedy Sketches & Stand-Up")]
    ComedySketchesAndStandUp,
    
    [Description("Science Experiments & Explainers")]
    ScienceExperimentsAndExplainers,
    
    [Description("Documentary & History")]
    DocumentaryAndHistory,
    
    [Description("Book Reviews & Literature Analysis")]
    BookReviewsAndLiteratureAnalysis,
    
    [Description("Animation & Short Films")]
    AnimationAndShortFilms,
    
    [Description("Sports Highlights & Analysis")]
    SportsHighlightsAndAnalysis,
    
    [Description("Health & Wellness Advice")]
    HealthAndWellnessAdvice,
    
    [Description("Personal Vlogs & Day in the Life")]
    PersonalVlogsAndDayInTheLife,
    
    [Description("News & Current Events")]
    NewsAndCurrentEvents,
    
    [Description("Pet Care & Animal Videos")]
    PetCareAndAnimalVideos,
    
    [Description("Art & Painting Tutorials")]
    ArtAndPaintingTutorials,
    
    [Description("Filmmaking & Video Editing Techniques")]
    FilmmakingAndVideoEditingTechniques,
    
    [Description("Unboxing & Product Hauls")]
    UnboxingAndProductHauls,
    
    [Description("Mystery & Paranormal Stories")]
    MysteryAndParanormalStories,
    
    [Description("Career & Business Advice")]
    CareerAndBusinessAdvice,
    
    [Description("Language Learning & Cultural Exchange")]
    LanguageLearningAndCulturalExchangex
}

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute == null ? value.ToString() : attribute.Description;
    }
}