import { Button, Container, Flex, Grid, Group, TextInput, Title, Text } from '@mantine/core';
import React from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { useRegisterUserMutation } from '../../state/auth/api';
import { object, string, number, date } from 'yup';

const SignUp = () => {
	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();

	const [registerUser, { isLoading, isSuccess, isError }] = useRegisterUserMutation();

	const schema = object({
		username: string()
			.required('Username is required')
			.max(15, 'Username must be at most 15 characters long')
			.min(3, 'Username must be at least 3 characters long'),
	});

	const onSubmit = async (data) => {
		try {
			await schema.validate(data);

			await registerUser(data);
		} catch (err) {
			console.log(err);
		}
	};

	return (
		<Container>
			<Grid>
				<Grid.Col span={6}>
					<Flex mih='100vh' align='start' justify='center' direction='column'>
						<Title order={1} mb='lg'>
							Spendrly
						</Title>
						<Title order={2}>Sign Up</Title>
						<Text mb='lg'>Please enter your details</Text>
						<form
							style={{
								width: '100%',
							}}
							onSubmit={handleSubmit(onSubmit)}
						>
							<TextInput {...register('username')} w='100%' label='Username' placeholder='johndoe' />

							<TextInput {...register('email')} w='100%' my={'md'} label='Email' placeholder='example@email.com' />

							<TextInput {...register('firstName')} w='100%' label='First Name' placeholder='John' />
							<TextInput {...register('lastName')} w='100%' my={'md'} label='Last Name' placeholder='Doe' />

							<TextInput {...register('password')} w='100%' label='Password' placeholder='******' type='password' />

							<TextInput {...register('confirmPassword')} mt={'md'} w='100%' label='Confirm Password' type='password' />

							<Group justify='start' my='xl'>
								<Button disabled={isLoading} type='submit'>
									Sign Up
								</Button>
							</Group>
						</form>
						<span>
							Already have an account ?{' '}
							<Link
								to={'/sign-in'}
								style={{
									color: '#07C',
								}}
							>
								Sign In
							</Link>
						</span>
					</Flex>
				</Grid.Col>
				<Grid.Col span={6}></Grid.Col>
			</Grid>
		</Container>
	);
};

export default SignUp;
