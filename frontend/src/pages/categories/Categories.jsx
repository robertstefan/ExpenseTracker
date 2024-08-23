import React from 'react';
import { Link } from 'react-router-dom';
import { notifications } from '@mantine/notifications';
import { ActionIcon, Button, Group, Table, Title } from '@mantine/core';
import { IconTrash } from '@tabler/icons-react';
import { useDeleteCategoryMutation, useGetCategoriesQuery } from '../../state/category/api';

const Categories = () => {
	const { data: categories = [], isLoading: isLoadingCategories } = useGetCategoriesQuery();
	const [deleteCategory] = useDeleteCategoryMutation();

	return (
		<div>
			<Group justify='space-between'>
				<Title>Categories</Title>
				<Link to='/category/new'>
					<Button>Add</Button>
				</Link>
			</Group>
			{isLoadingCategories && <p>loading...</p>}
			<Table striped highlightOnHover>
				<Table.Thead>
					<Table.Tr>
						<Table.Th>Id</Table.Th>
						<Table.Th>Name</Table.Th>
						<Table.Th>Actions</Table.Th>
					</Table.Tr>
				</Table.Thead>
				<Table.Tbody>
					{categories.map((category) => (
						<Table.Tr key={category.id}>
							<Table.Td>
								<Link to={`/category/${category.id}`}>{category.id}</Link>
							</Table.Td>
							<Table.Td>{category.name}</Table.Td>
							<Table.Td>
								<ActionIcon
									onClick={async () => {
										await deleteCategory(category.id);
										notifications.show({
											title: 'Category Deleted',
											message: `Category ${category.name} was deleted!`,
											position: 'bottom-right',
										});
									}}
								>
									<IconTrash size={16} />
								</ActionIcon>
							</Table.Td>
						</Table.Tr>
					))}
				</Table.Tbody>
			</Table>
		</div>
	);
};

export default Categories;
