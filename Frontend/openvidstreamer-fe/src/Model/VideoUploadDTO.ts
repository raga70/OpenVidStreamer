

export enum VideoCategory {
    Other = "Other",
    Fun = "Fun",
    Educational = "Educational",
    Music = "Music",
    News = "News",
    Sports = "Sports",
    DIYProjectsAndCrafts = "DIY Projects & Crafts",
    TechnologyAndGadgetsReviews = "Technology & Gadgets Reviews",
    CookingAndRecipes = "Cooking & Recipes",
    FitnessAndExerciseRoutines = "Fitness & Exercise Routines",
    EducationalTutorials = "Educational Tutorials",
    TravelVlogs = "Travel Vlogs",
    GamingAndEsports = "Gaming & Esports",
    FashionAndBeautyTips = "Fashion & Beauty Tips",
    MusicCoversAndOriginals = "Music Covers & Originals",
    ComedySketchesAndStandUp = "Comedy Sketches & Stand-Up",
    ScienceExperimentsAndExplainers = "Science Experiments & Explainers",
    DocumentaryAndHistory = "Documentary & History",
    BookReviewsAndLiteratureAnalysis = "Book Reviews & Literature Analysis",
    AnimationAndShortFilms = "Animation & Short Films",
    SportsHighlightsAndAnalysis = "Sports Highlights & Analysis",
    HealthAndWellnessAdvice = "Health & Wellness Advice",
    PersonalVlogsAndDayInTheLife = "Personal Vlogs & Day in the Life",
    NewsAndCurrentEvents = "News & Current Events",
    PetCareAndAnimalVideos = "Pet Care & Animal Videos",
    ArtAndPaintingTutorials = "Art & Painting Tutorials",
    FilmmakingAndVideoEditingTechniques = "Filmmaking & Video Editing Techniques",
    UnboxingAndProductHauls = "Unboxing & Product Hauls",
    MysteryAndParanormalStories = "Mystery & Paranormal Stories",
    CareerAndBusinessAdvice = "Career & Business Advice",
    LanguageLearningAndCulturalExchange = "Language Learning & Cultural Exchange",
}

export const VideoCategoryNames = {
    Other: "Other",
    Fun: "Fun",
    Educational: "Educational",
    Music: "Music",
    News: "News",
    Sports: "Sports",
    DIYProjectsAndCrafts: "DIY Projects & Crafts",
    TechnologyAndGadgetsReviews: "Technology & Gadgets Reviews",
    CookingAndRecipes: "Cooking & Recipes",
    FitnessAndExerciseRoutines: "Fitness & Exercise Routines",
    EducationalTutorials: "Educational Tutorials",
    TravelVlogs: "Travel Vlogs",
    GamingAndEsports: "Gaming & Esports",
    FashionAndBeautyTips: "Fashion & Beauty Tips",
    MusicCoversAndOriginals: "Music Covers & Originals",
    ComedySketchesAndStandUp: "Comedy Sketches & Stand-Up",
    ScienceExperimentsAndExplainers: "Science Experiments & Explainers",
    DocumentaryAndHistory: "Documentary & History",
    BookReviewsAndLiteratureAnalysis: "Book Reviews & Literature Analysis",
    AnimationAndShortFilms: "Animation & Short Films",
    SportsHighlightsAndAnalysis: "Sports Highlights & Analysis",
    HealthAndWellnessAdvice: "Health & Wellness Advice",
    PersonalVlogsAndDayInTheLife: "Personal Vlogs & Day in the Life",
    NewsAndCurrentEvents: "News & Current Events",
    PetCareAndAnimalVideos: "Pet Care & Animal Videos",
    ArtAndPaintingTutorials: "Art & Painting Tutorials",
    FilmmakingAndVideoEditingTechniques: "Filmmaking & Video Editing Techniques",
    UnboxingAndProductHauls: "Unboxing & Product Hauls",
    MysteryAndParanormalStories: "Mystery & Paranormal Stories",
    CareerAndBusinessAdvice: "Career & Business Advice",
    LanguageLearningAndCulturalExchange: "Language Learning & Cultural Exchange",
};


export type VideoUploadDTO = {
    title: string;
    description: string;
    category: VideoCategory;
};


export function GetVideoCategoryEnumIndex(value: VideoCategory): number {
    const enumValues = Object.values(VideoCategory); // Create an array of enum values
    return enumValues.indexOf(value);        // Return the index of the value
}