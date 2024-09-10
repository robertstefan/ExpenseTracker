import { Button, Container, Flex, Grid, Group, TextInput, Title, Text } from '@mantine/core';
import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { useLoginUserMutation } from '../../state/auth/api';
import { setCredentials } from '../../state/auth/authSlice';
import { useDispatch } from 'react-redux';
const SignIn = () => {
	const [user, setUser] = useState();
	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();

	const [loginUser, { isLoading, isSuccess, isError, error }] = useLoginUserMutation();

	const dispatch = useDispatch();

	const onSubmit = async (data) => {
		try {
			const result = await loginUser(data).unwrap();

			dispatch(setCredentials({ ...result, user }));
		} catch (err) {}
	};

	return (
		<Container>
			<Grid>
				<Grid.Col span={6}>
					<Flex mih='100vh' align='start' justify='center' direction='column'>
						<Title order={1} mb='lg'>
							Spendrly
						</Title>
						<Title order={2}>Sign In</Title>
						<Text mb='lg'>Please enter your credentials</Text>
						<form
							style={{
								width: '100%',
							}}
							onSubmit={handleSubmit(onSubmit)}
						>
							<TextInput {...register('email')} w='100%' label='Email' placeholder='example@email.com' />
							<TextInput {...register('password')} w='100%' mt='md' label='Password' placeholder='******' type='password' />

							<Group justify='start' my='xl'>
								<Button disabled={isLoading} type='submit'>
									Sign In
								</Button>
							</Group>
						</form>
						<span>
							Don't have an account ?{' '}
							<Link
								to={'/sign-up'}
								style={{
									color: '#07C',
								}}
							>
								Sign Up
							</Link>
						</span>
					</Flex>
				</Grid.Col>
				<Grid.Col span={6}></Grid.Col>
			</Grid>
		</Container>
	);
};

export default SignIn;
