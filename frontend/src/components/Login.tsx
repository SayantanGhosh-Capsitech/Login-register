import { Form, Input, Button, Typography, Card, Flex } from "antd";
import  Logo  from "../components/Logo";
import { NavLink, useNavigate } from "react-router-dom";
import type { LoginType } from "../Types/User";
import API from "../api/Api";
 
const { Title } = Typography;
 
const Login = () => {
   const navigate = useNavigate();
   const [form] = Form.useForm();

    const onFinish = async (values:LoginType ) => {
    try {
      console.log('Sending Login data:', values);
      const response = await API.post("/login", values);
      console.log("Login successful:", response.data);
      navigate("/home");
    } catch (err: any) {
      console.error("Login failed:", err.response?.data || err.message);
    }
 
  };
    
  return (
    <>
      <div
        style={{
          display: "flex",
          height: "100vh",
          justifyContent: "center",
          alignItems: "center",
          backgroundColor: '#F5F5F5'
        }}
      >
        <Card
          style={{
            width: 400,
            padding: 24,
            backgroundColor: "white",
            border: "none",
            borderRadius: 12,
          }}
        >
          <Flex justify="center" align=""><Logo /></Flex>
          <Title level={4} style={{ textAlign: "center", marginTop: 20 }}>
            Login to your account
          </Title>
          <Form
            form={form}
            name="login_form"
            initialValues={{ remember: true }}
            onFinish={onFinish}
            layout="vertical"
            disabled={false}
          >
            <Form.Item
              name="email"
              label="Email"
              rules={[{ required: true, message: "Please enter your email!" }]}
            >
              <Input placeholder="Email" />
            </Form.Item>
 
            <Form.Item
              name="password"
              label="Password"
              rules={[
                { required: true, message: "Please enter your password!" },
              ]}
            >
              <Input.Password placeholder="Password" />
            </Form.Item>
 
            <Form.Item>
              <Button type="primary" htmlType="submit" block>
                Login
              </Button>
            </Form.Item>
          </Form>
        </Card>
      </div>
    </>
  );
};
 
export default Login;
 
