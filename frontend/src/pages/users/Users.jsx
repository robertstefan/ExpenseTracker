import { ActionIcon, Button, Group, Table, Title } from '@mantine/core';
import { IconTrash } from '@tabler/icons-react';
import { Link } from 'react-router-dom';
import { useDeleteUserMutation, useGetUsersQuery } from '../../state/user/api';
import { notifications } from '@mantine/notifications';

const Users = () => {
	const { data: users = [] } = useGetUsersQuery();
	const [deleteUser] = useDeleteUserMutation();
	console.log(users);
	return (
		<div>
			<Group justify='space-between'>
				<Title>Users</Title>
				<Link to='/users/new'>
					<Button>Add</Button>
				</Link>
			</Group>

			<Table striped highlightOnHover>
				<Table.Thead>
					<Table.Tr>
						<Table.Th>Id</Table.Th>
						<Table.Th>Name</Table.Th>
						<Table.Th>Actions</Table.Th>
					</Table.Tr>
				</Table.Thead>
				<Table.Tbody>
					{users.map((user) => (
						<Table.Tr key={user.id}>
							<Table.Td>
								<Link to={`/users/${user.id}`}>{user.id}</Link>
							</Table.Td>
							<Table.Td>{user.username}</Table.Td>
							<Table.Td>
								<ActionIcon
									onClick={async () => {
										const result = await deleteUser(user.id);
										notifications.show({
											title: 'User Deleted',
											message: `User ${user.id} has been deleted!`,
											position: 'bottom-right',
										});
										console.log(result);
									}}
								>
									<IconTrash size={15} />
								</ActionIcon>
							</Table.Td>
						</Table.Tr>
					))}
				</Table.Tbody>
			</Table>
		</div>
	);
};

export default Users;
