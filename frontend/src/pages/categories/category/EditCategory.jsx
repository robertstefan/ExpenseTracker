import { useNavigate, useParams } from 'react-router-dom';
import { useDeleteCategoryMutation, useGetCategoryQuery } from '../../../state/category/api';
import { Button, Group, Title } from '@mantine/core';
import { notifications } from '@mantine/notifications';
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
			</Group>
			<div>Category Id: {category.id}</div>
			<div>Category Name: {category.categoryName}</div>
		</div>
	);
};

export default EditCategory;
