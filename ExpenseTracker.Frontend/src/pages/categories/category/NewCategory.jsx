import { Button, TextInput, Title } from '@mantine/core';
import React from 'react';
import { useForm } from 'react-hook-form';
import { useCreateCategoryMutation } from '../../../state/category/api';
import { useNavigate } from 'react-router-dom';
export default function NewCategory() {
	const navigate = useNavigate();
	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();

	const [createCategory, resultCreatedCategory] = useCreateCategoryMutation();

	const onSubmit = async (data) => {
		await createCategory(data);
		navigate('/category');
	};

	return (
		<div>
			<Title>New Category</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<TextInput
					{...register('name', {
						required: true,
						maxLength: 20,
					})}
					label='Name'
				/>
				<Button type='submit' mt='md' disabled={resultCreatedCategory?.isLoading}>
					Submit
				</Button>
			</form>
		</div>
	);
}
