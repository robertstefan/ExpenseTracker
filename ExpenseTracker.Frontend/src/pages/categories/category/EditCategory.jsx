import React from 'react';
import { useParams } from 'react-router-dom';
import { useGetCategoryQuery } from '../../../state/category/api';
import { Button, TextInput, Title } from '@mantine/core';
import { useForm } from 'react-hook-form';
export default function EditCategory() {
	const { id } = useParams();

	const { data: category = {}, isLoading, isError } = useGetCategoryQuery(id);
	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();

	const onSubmit = async (data) => {
		await createCategory(data);
	};

	if (isError || isLoading) {
		return <></>;
	}

	return (
		<>
			<Title>Category</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<TextInput
					{...register('name', {
						required: true,
						maxLength: 20,
					})}
					label='Name'
				/>
				<Button type='submit' mt='md'>
					Submit
				</Button>
			</form>
		</>
	);
}
