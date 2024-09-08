import { useNavigate } from 'react-router-dom';
import { useCreateUserMutation } from '../../../state/user/api';
import { useForm } from 'react-hook-form';
import { notifications } from '@mantine/notifications';
import { Button, TextInput, Title } from '@mantine/core';

const NewUser = () => {
	const navigate = useNavigate();

	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();
	const [createUser, resultCreateUser] = useCreateUserMutation();

	const onSubmit = async (data) => {
		await createUser(data);
		notifications.show({
			title: 'User Added',
			message: `User ${data.Username} was added`,
			position: 'bottom-right',
		});
		console.log(data);
		navigate('/users');
	};

	return (
		<div>
			<Title>New User</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<TextInput
					{...register('Username', { required: 'Username is a required field!' })}
					label='Username'
					withAsterisk
					error={errors.name?.message}
				/>
				<TextInput
					{...register('Email', { required: 'Email is a required field!' })}
					label='Email Address'
					withAsterisk
					error={errors.name?.message}
				/>
				<TextInput
					{...register('PasswordHash', { required: 'Password Hash is a required field!' })}
					label='Password Hash'
					withAsterisk
					error={errors.name?.message}
				/>
				<TextInput
					{...register('LastName', { required: 'LastName is a required field!' })}
					label='Last Name'
					withAsterisk
					error={errors.name?.message}
				/>
				<TextInput
					{...register('FirstName', { required: 'FirstName is a required field!' })}
					label='First Name'
					withAsterisk
					error={errors.name?.message}
				/>
				<Button type='submit' mt='md' disabled={resultCreateUser?.isLoading}>
					Submit
				</Button>
			</form>
		</div>
	);
};

export default NewUser;
