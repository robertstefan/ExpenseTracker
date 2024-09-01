import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { Button, TextInput, Title } from '@mantine/core';
import { notifications } from '@mantine/notifications';
import { useCreateCategoryMutation } from '../../../state/category/api';

const NewCategory = () => {
	const navigate = useNavigate();

	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();

	const [createCategory, resultCreateCategory] = useCreateCategoryMutation();

	const onSubmit = async (data) => {
		await createCategory(data);

		notifications.show({
			title: 'Category Added',
			message: `Category ${data.name} was added!`,
			position: 'bottom-right',
		});

		navigate('/category');
	};

	return (
		<div>
			<Title>New Category</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<TextInput
					{...register('name', { required: 'Category name is a required field!' })}
					label='Name'
					withAsterisk
					error={errors.name?.message}
				/>
				<Button type='submit' mt='md' disabled={resultCreateCategory?.isLoading}>
					Submit
				</Button>
			</form>
		</div>
	);
};

export default NewCategory;
