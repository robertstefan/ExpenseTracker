import { useNavigate, useParams } from 'react-router-dom';
import { useDeleteCategoryMutation, useGetCategoryQuery, useUpdateCategoryMutation } from '../../../state/category/api';
import { Button, Group, Modal, TextInput, Title } from '@mantine/core';
import { notifications } from '@mantine/notifications';
import { useDisclosure } from '@mantine/hooks';
import { useForm } from 'react-hook-form';
const EditCategory = () => {
	const { id } = useParams();
	const [opened, { open, close }] = useDisclosure(false);
	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();
	const [updateCategory, resultUpdateCategory] = useUpdateCategoryMutation();
	const navigate = useNavigate();

	const { data: category = {} } = useGetCategoryQuery(id);
	const [deleteCategory] = useDeleteCategoryMutation();

	const onSubmit = async (data) => {
		await updateCategory({ id, ...data });
		notifications.show({
			title: 'Category updated',
			message: `Category ${id} was updated`,
			position: 'bottom-right',
		});
		console.log(data);
		console.log(id);
	};

	return (
		<div>
			<Group justify='space-between'>
				<Title mr='auto'>Category</Title>
				<Button
					color='red'
					onClick={async () => {
						await deleteCategory(id);
						navigate('/categories');
						notifications.show({
							title: 'Category Deleted',
							message: `Category ${category.categoryName} has been deleted!`,
							position: 'bottom-right',
						});
					}}
				>
					Delete
				</Button>
				<Button color='green' onClick={open}>
					Edit
				</Button>
			</Group>
			<div>Category Id: {category.id}</div>
			<div>Category Name: {category.categoryName}</div>

			<Modal opened={opened} onClose={close} title='Edit Category' centered>
				<form onSubmit={handleSubmit(onSubmit)}>
					<TextInput
						{...register('CategoryName', { required: 'Category name is a required field!' })}
						label='Category Name'
						withAsterisk
						error={errors.name?.message}
					/>
					<Button type='submit' mt='md' disabled={resultUpdateCategory?.isLoading}>
						Submit
					</Button>
				</form>
			</Modal>
		</div>
	);
};

export default EditCategory;
