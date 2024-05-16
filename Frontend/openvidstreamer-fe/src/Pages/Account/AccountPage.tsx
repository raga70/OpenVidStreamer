import PaymentComponent from "./PaymentComponent.tsx";
import {useEffect, useState} from "react";
import axios from "axios";
import {ApiServerBaseUrl, StripePublishableKey, SubscriptionCost} from "../../../configProvider.ts";
import {dispatch, useStoreState} from "../../persistenceProvider.ts";
import {AccountDTO} from "../../Model/AccountDTO.ts";
import toast from "react-hot-toast";
import {loadStripe} from "@stripe/stripe-js";
import {Elements} from "@stripe/react-stripe-js";
import {Box, Button, Center, Flex, Heading, Text, VStack} from '@chakra-ui/react';
import {data} from "autoprefixer";

const AccountPage = () => {

    const [account, setAccount] = useState<AccountDTO | null>(null)

    const [isThereActiveSubscriptiom, setIsThereActiveSubscriptiom] = useState(false)
    
    const authToken = useStoreState("authToken");

    useEffect(() => {
        const work = async () => {
            const result = await axios.get(ApiServerBaseUrl() + "/account/getAccount", {
                headers: {
                    Authorization: `Bearer ${authToken}`
                }
            })
            setAccount(result.data)

            try {
            if (Date.parse(result.data.subscriptionValidUntil) > new Date()) {
                setIsThereActiveSubscriptiom(true)
            }
            }catch (e) {
                //exception might trigger if subscriptionValidUntil is null , so we just ignore it
            }
            console.log("account:",result.data)
        }
        work();
    }, []);
    
    
    const handleActivateSubscription = async () => {
        
        if (account?.balance < SubscriptionCost()){
            toast.error("Not enough funds, subscription costs: " + SubscriptionCost() +"€")
            return;
        }
        
        const result = await axios.post(ApiServerBaseUrl() + "/account/activateSubscription", {}, {
            headers: {
                Authorization: `Bearer ${authToken}`
            }
        })
        toast.success("Subscription activated, \n you will be logged out to refresh your account data, \n please wait ...")

        dispatch({type: 'setAuthToken', authToken: null});
        setTimeout(() => {
            window.location.reload();
        }, 3000)
        
        
    }

    const stripePromise = loadStripe(StripePublishableKey());

    const aappearance = {
        theme: 'flat',
    }
    
    return (
        account ? (
            <Center height="100vh">
                <Flex direction="row" align="stretch" justify="center" gap="20px">
                    <Box backgroundColor={"#fff"} flex="1" p={5} borderWidth="1px" borderRadius="lg" boxShadow="lg">
                        <VStack spacing={4} align="stretch">
                            <Heading as="h2" size="md">Account:</Heading>
                            <Text as="h3" fontWeight={500} fontSize="md">Email: {account.email}</Text>
                            <Text as="h3" fontWeight={500} fontSize="md">Balance: {account.balance} €</Text>
                            {isThereActiveSubscriptiom ? (
                                <Text as="h4" fontWeight={500} fontSize="md">Active Subscription until: {new Date(account.subscriptionValidUntil).toLocaleDateString()}</Text>
                            ) : (
                                <>
                                    <Text as="h3" fontSize="lg">No Active Subscription</Text>
                                    <Button colorScheme="blue" onClick={handleActivateSubscription}>
                                        Activate Subscription
                                    </Button>
                                </>
                            )}
                        </VStack>
                    </Box>
                    <Box backgroundColor={"#fff"} flex="1" p={5} borderWidth="1px" borderRadius="lg" boxShadow="lg">
                        <VStack spacing={4} align="stretch">
                            <Heading as="h2" size="md">Add Funds:</Heading>
                            <Elements  stripe={stripePromise}>
                                <PaymentComponent />
                            </Elements>
                        </VStack>
                    </Box>
                </Flex>
            </Center>
        ) : null
    );
};

export default AccountPage;
