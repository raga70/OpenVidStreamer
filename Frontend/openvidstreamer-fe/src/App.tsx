import './App.css';

import {BrowserRouter as Router, Route, Routes,} from "react-router-dom";
import LoginPageComp from "./Pages/Login/LoginPage.tsx";
import UploadPage from "./Pages/Upload/UploadPage.tsx";
import HomePage from "./Pages/VideoRecomendations/HomePage.tsx";
import {useEffect, useState} from "react";
import {useStoreState} from "./persistenceProvider.ts";
import {loadConfig} from "../configProvider.ts";
import VideoPlayer from "./Pages/VideoPlayer.tsx";
import Navbar from "./Pages/NavBar.tsx";
import {Toaster} from "react-hot-toast";
import AccountPage from "./Pages/Account/AccountPage.tsx";

import {ChakraProvider, extendTheme} from "@chakra-ui/react";
import PaymentProcessedPage from "./Pages/Account/PaymentProcessedPage.tsx";
import Search from "./Pages/Search.tsx";


function App() {

    const authToken = useStoreState("authToken");
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false)

    const chackraConfig = {
        initialColorMode: 'dark',  // Set the initial color mode to dark
        useSystemColorMode: false, // Optionally allow users to use their system color mode
    };
    const theme = extendTheme({ chackraConfig });
    
    useEffect(() => {
        const loadConf = async () => {
            await loadConfig()
        }
    
        loadConf();
        console.log(authToken)
    if (authToken != null && authToken != "") {
        console.log("authToken is not null")
        setIsLoggedIn(true)
    }
    }, []);

    return (
        <>
            <ChakraProvider theme={theme}>
            {!isLoggedIn ? <LoginPageComp/> :
                <Router>
                    <Toaster />
                    <Navbar />
                    <Routes> 
                        <Route path="/login" element={<LoginPageComp/>}/> 
                        <Route path="/upload" element={<UploadPage/>}/> 
                        <Route path="/home" element={<HomePage/>}/>
                        <Route path="/video-player" element={<VideoPlayer />} />
                        <Route path="/account" element={<AccountPage/>}/>
                        <Route path="/paymentProcessed" element={<PaymentProcessedPage/>}/>
                        <Route path="/search" element={<Search/>}/>
                        <Route path="/" element={<HomePage/>}/>
                    </Routes>
                </Router>
            }
            </ChakraProvider>
        </>
    );
}

export default App;
