import LoginPage, { Logo, Password, Reset, Submit, Title, Username} from "@react-login-page/page1";
import {Button, Modal} from 'flowbite-react';
import TermsAndConditionsText from "./TermsAndConditionsText.tsx";
import {useState} from "react";
import axios from "axios";
import {ApiServerBaseUrl} from "../../../configProvider.ts";
import {LoginResponse} from "../../Model/LoginResponse.ts";
import {dispatch} from "../../../persistenceProvider.ts";
import toast, {Toaster} from "react-hot-toast";


const LoginPageComp = () => {

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isGdprModalOpen, setIsGdprModalOpen] = useState(false);
  //  const [isUserConsented, setIsUserConsented] = useState(false)
    const handleRegister = async () => {
      
        
     
      const resp:LoginResponse = await  axios.post(ApiServerBaseUrl()+"/account/register", {email: username, passwordUnhashed: password}).catch((e) => {if (e.response.status == 442) {toast.error("Email already in use")} else {toast.error("Registration failed")}})
            
       
        
        // toast.success("Registration successful")
        dispatch({type: 'setAuthToken', authToken: resp.item2})
        dispatch({type: 'setAccountData', accountData: resp.item1})
        window.location.href = "/home"
        
    }
    const handleLogin = async () => {
        console.log("handling  login")
        const resp = await  axios.post(ApiServerBaseUrl()+"/account/login", {email: username, passwordUnhashed: password})
       console.log(resp)
        if (resp.data == null || resp.data == "") {
            toast.error("Login failed (incorrect details)")
            return
        }
        dispatch({type: 'setAuthToken', authToken: resp.data.item2})
        dispatch({type: 'setAccountData', accountData: resp.data.item1})
      window.location.reload()
    }
    
    return (
        <>
            <Toaster/>
            <div style={{height:"100vh", width:"100vw", overflow:"hidden"}}>
                <Modal style={{color:"black"}} show={isGdprModalOpen} onClose={() => setIsGdprModalOpen(false)}>
                    <Modal.Header>Terms and Conditions</Modal.Header>
                    <Modal.Body>
                        <TermsAndConditionsText/>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button  style={{backgroundColor:"blue"}} onClick={() => handleRegister()}>I accept</Button>
                      
                    </Modal.Footer>
                </Modal>
                <LoginPage>
                    <Username value={username} onChange={(e) => {
                        setUsername(e.target.value)
                    }}  placeholder={"Email"} name="userUserName"/>
                    <Password value={password} onChange={(e) => {
                        setPassword(e.target.value)
                    }} placeholder="Password" name="userPassword"/>
                    <Submit onClick={handleLogin}>Login</Submit>
                    <Reset onClick={ ()=>setIsGdprModalOpen(true)}>Register</Reset>
                    <Title/>
                    <Logo>
                        <p>logoPlaceholder</p>
                    </Logo>
                    <Title>OpenVidStreamer</Title>
                </LoginPage>

            </div>
        </>)
}

export default LoginPageComp;