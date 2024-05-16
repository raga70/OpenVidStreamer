import LoginPage, { Logo, Password, Reset, Submit, Title, Username} from "@react-login-page/page1";
import { Modal} from 'flowbite-react';
import TermsAndConditionsText from "./TermsAndConditionsText.tsx";
import {useState} from "react";
import axios from "axios";
import {ApiServerBaseUrl} from "../../../configProvider.ts";
import {LoginResponse} from "../../Model/LoginResponse.ts";
import {dispatch} from "../../persistenceProvider.ts";
import toast, {Toaster} from "react-hot-toast";
import {
    Flex,
    Box,
    FormControl,
    FormLabel,
    Input,
    Checkbox,
    Stack,
    Link,
    Button,
    Heading,
    Text,
    useColorModeValue,
} from '@chakra-ui/react';

const LoginPageComp = () => {

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isGdprModalOpen, setIsGdprModalOpen] = useState(false);
  //  const [isUserConsented, setIsUserConsented] = useState(false)
    const handleRegister = async () => {
      
        
     
      const resp = await  axios.post(ApiServerBaseUrl()+"/account/register", {email: username, passwordUnhashed: password}).catch((e) => {if (e.response.status == 442) {toast.error("Email already in use")} else {toast.error("Registration failed")}})
        
        const data:LoginResponse = resp.data;
        
      
        
        // toast.success("Registration successful")
        dispatch({type: 'setAuthToken', authToken: data.item2})
        dispatch({type: 'setAccountData', accountData: data.item1})
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

            {/*<div style={{height:"100vh", width:"100vw", overflow:"hidden"}}>*/}
                <Modal style={{color:"black"}} show={isGdprModalOpen} onClose={() => setIsGdprModalOpen(false)}>
                    <Modal.Header>Terms and Conditions</Modal.Header>
                    <Modal.Body>
                        <TermsAndConditionsText/>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button
                            bg={'blue.400'}
                            color={'white'}
                            _hover={{
                                bg: 'blue.500',
                            }}
                            onClick={() => handleRegister()}>I accept</Button>
                      
                    </Modal.Footer>
                </Modal>
            <Flex
                minH={'100vh'}
                align={'center'}
                justify={'center'}
                bg={useColorModeValue('gray.50', 'gray.800')}>
                <Stack spacing={8} mx={'auto'} maxW={'lg'} py={12} px={6}>
                    <Stack align={'center'}>
                        <Heading fontSize={'4xl'}>Sign/Register</Heading>
                        <Text fontSize={'lg'} color={'gray.600'}>
                            to enjoy all of our cool <Link color={'blue.400'}>Videos</Link> ✌️
                        </Text>
                    </Stack>
                    <Box
                        rounded={'lg'}
                        bg={useColorModeValue('white', 'gray.700')}
                        boxShadow={'lg'}
                        p={8}>
                        <Stack spacing={4}>
                            <FormControl id="email">
                                <FormLabel color={"black"}>Email address</FormLabel>
                                <Input value={username} onChange={(e) => {
                                    setUsername(e.target.value)
                                }} type="email" />
                            </FormControl>
                            <FormControl id="password">
                                <FormLabel color={"black"}>Password</FormLabel>
                                <Input value={password} onChange={(e) => {
                                    setPassword(e.target.value)
                                }} type="password" />
                            </FormControl>
                            <Stack spacing={10}>
                                <Stack
                                    direction={{ base: 'column', sm: 'row' }}
                                    align={'start'}
                                    justify={'space-between'}>
                                <Button
                                    bg={'blue.400'}
                                    color={'white'}
                                    _hover={{
                                        bg: 'blue.500',
                                    }}
                                    onClick={handleLogin}
                                >
                                    Sign in
                                </Button>
                                    <Button
                                        bg={'blue.400'}
                                        color={'white'}
                                        _hover={{
                                            bg: 'blue.500',
                                        }}
                                        onClick={ ()=>setIsGdprModalOpen(true)}
                                    
                                    >
                                        Register
                                    </Button>
                                </Stack>
                            </Stack>
                        </Stack>
                    </Box>
                </Stack>
            </Flex>
            
            
            {/*<div style={{height:"100vh", width:"100vw", overflow:"hidden"}}>*/}
            {/*    <Modal style={{color:"black"}} show={isGdprModalOpen} onClose={() => setIsGdprModalOpen(false)}>*/}
            {/*        <Modal.Header>Terms and Conditions</Modal.Header>*/}
            {/*        <Modal.Body>*/}
            {/*            <TermsAndConditionsText/>*/}
            {/*        </Modal.Body>*/}
            {/*        <Modal.Footer>*/}
            {/*            <Button  style={{backgroundColor:"blue"}} onClick={() => handleRegister()}>I accept</Button>*/}
            {/*          */}
            {/*        </Modal.Footer>*/}
            {/*    </Modal>*/}
            {/*    <LoginPage>*/}
            {/*        <Username value={username} onChange={(e) => {*/}
            {/*            setUsername(e.target.value)*/}
            {/*        }}  placeholder={"Email"} name="userUserName"/>*/}
            {/*        <Password value={password} onChange={(e) => {*/}
            {/*            setPassword(e.target.value)*/}
            {/*        }} placeholder="Password" name="userPassword"/>*/}
            {/*        <Submit onClick={handleLogin}>Login</Submit>*/}
            {/*        <Reset onClick={ ()=>setIsGdprModalOpen(true)}>Register</Reset>*/}
            {/*        <Title/>*/}
            {/*        <Logo>*/}
            {/*            <p>logoPlaceholder</p>*/}
            {/*        </Logo>*/}
            {/*        <Title>OpenVidStreamer</Title>*/}
            {/*    </LoginPage>*/}
            
            {/*</div>*/}
        </>)
}

export default LoginPageComp;