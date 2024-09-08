import { Button, TextInput, Title } from '@mantine/core';
import { useForm } from 'react-hook-form';
import { useCreateCategoryMutation } from '../../../state/category/api';
import { notifications } from '@mantine/notifications';
import { useNavigate } from 'react-router-dom';
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
			message: `Category ${data.categoryName} was added`,
			position: 'bottom-right',
		});
		console.log(data);
		navigate('/categories');
	};

	return (
		<div>
			<Title>New Category</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<TextInput
					{...register('categoryName', { required: 'Category name is a required field!' })}
					label='Category Name'
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
