import unittest
from httpx import Client

class LocationsTest(unittest.TestCase):
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})


if __name__ == '__main__':
    unittest.main()
