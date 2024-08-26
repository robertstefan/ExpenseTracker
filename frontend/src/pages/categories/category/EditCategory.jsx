import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Button, Group, Title } from '@mantine/core';
import { notifications } from '@mantine/notifications';
import { useDeleteCategoryMutation, useGetCategoryQuery } from '../../../state/category/api';

const EditCategory = () => {
	const { id } = useParams();
	const navigate = useNavigate();
	const { data: category = {} } = useGetCategoryQuery(id);
	const [deleteCategory] = useDeleteCategoryMutation();

	return (
		<div>
			<Group justify='space-between'>
				<Title>Category</Title>
				<Button
					onClick={async () => {
						await deleteCategory(id);
						navigate('/category');

						notifications.show({
							title: 'Category Deleted',
							message: `Category ${category.name} was deleted!`,
							position: 'bottom-right',
						});
					}}
				>
					Delete
				</Button>
			</Group>
			<div>Category Id: {category.id}</div>
			<div>Category Name: {category.name}</div>
		</div>
	);
};

export default EditCategory;
