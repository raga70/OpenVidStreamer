import  { useState } from 'react';
import axios from 'axios';

import {  CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import {ApiServerBaseUrl} from "../../../configProvider.ts";
import {useStoreState} from "../../persistenceProvider.ts";
import { Box, Button, FormControl, FormLabel, Input, Text, VStack } from '@chakra-ui/react';
import toast from "react-hot-toast";


// Load your Stripe publishable key from configuration or environment variables


const PaymentComponent = () => {
    const [amount, setAmount] = useState('');
    const [paymentStatus, setPaymentStatus] = useState('');
    const stripe = useStripe();
    const elements = useElements();

    const authToken = useStoreState("authToken");


  
    
    const handleSubmit = async (event) => {
        event.preventDefault();
        if (!stripe || !elements) {
            return; // Make sure Stripe.js has loaded
        }

        const processingTransactionToastId = toast.loading('Processing Transaction...');
       
        
        const cardElement = elements.getElement(CardElement);
        const {error, paymentMethod} = await stripe.createPaymentMethod({
            type: 'card',
            card: cardElement,
        });

        if (error) {
            console.log('[error]', error);
            setPaymentStatus(error.message);
            toast.dismiss(processingTransactionToastId);
            return;
        }

        try {
            const { data } = await axios.post(ApiServerBaseUrl()+'/account/Payment/processPayment', {
                StripeToken: paymentMethod.id, 
                Amount: parseFloat(amount),
            }, 
            {
                headers: {
                    Authorization: `Bearer ${authToken}`
                    
                } 
                }    
            
            );

            if (data.status === 'requires_action') {
                // Handle 3D Secure here
                const {error, paymentIntent} = await stripe.confirmCardPayment(data.clientSecret);
                if (error) {
                    setPaymentStatus(`Authentication failed: ${error.message}`);
                    toast.dismiss(processingTransactionToastId);
                    return;
                }
                try {
                    const confirmResult = await axios.post(ApiServerBaseUrl()+'/account/Payment/confirmPayment', {paymentIntentId:paymentIntent.id}, {
                        
                        headers: {
                        
                            Authorization: `Bearer ${authToken}`
                        }
                    });

                    setPaymentStatus(confirmResult.data.message);
                    if(confirmResult.data.status === 'succeeded') {
                    toast.dismiss(processingTransactionToastId);
                    toast.success("Payment successful")
                    setTimeout(() => {
                        window.location.reload();
                    }, 2000)}
                } catch (confirmError) {
                    setPaymentStatus('Payment failed: ' + confirmError.response.data.message);
                    toast.dismiss(processingTransactionToastId);
                }
            } else {
                setPaymentStatus(data.message);
                if(data.status === 'succeeded') {
                    toast.dismiss(processingTransactionToastId);
                    toast.success("Payment successful")
                    setTimeout(() => {
                        window.location.reload();
                    }, 2000)}
            }
            setAmount('');
        } catch (error) {
            toast.dismiss(processingTransactionToastId);
            setPaymentStatus('Payment failed: ' + error.response.data.message);
        }
    };

    return (

        // <Box p={4} maxW="sm" borderWidth="1px" borderRadius="lg" boxShadow="lg" m="20px auto">
            <form onSubmit={handleSubmit}>
                <VStack spacing={4}>
                    <FormControl isRequired>
                        <FormLabel color={'black'} htmlFor="amount">Amount</FormLabel>
                        <Input
                            color={'black'}
                            id="amount"
                            type="number"
                            value={amount}
                            onChange={(e) => setAmount(e.target.value)}
                            placeholder="Enter amount"
                        />
                    </FormControl>
                    <FormControl isRequired>
                        <FormLabel color={'black'}>Card Details</FormLabel>
                        
                        <CardElement options={{hidePostalCode:true}}  className="card-details" />
                    </FormControl>
                    <Button colorScheme="blue" size="md" type="submit" isDisabled={!stripe}>
                        Submit Payment
                    </Button>
                    {paymentStatus && <Text color={paymentStatus.includes('failed')  ? 'red.500' : 'green.500'}>{paymentStatus}</Text>}
                </VStack>
            </form>
        // </Box>
       
    );
};

export default PaymentComponent;
